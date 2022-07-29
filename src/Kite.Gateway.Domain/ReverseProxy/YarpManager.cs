using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Mapster;
using Yarp.ReverseProxy.Health;
using Consul;
using Volo.Abp.Uow;
using Serilog;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Shared.Enums;
using Kite.Gateway.Domain.Shared.Options;
using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Configuration;
using Volo.Abp.Domain.Services;

namespace Kite.Gateway.Domain.ReverseProxy
{
    internal class YarpManager : DomainService, IYarpManager
    {
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<RouteTransform> _routeTransformRepository;
        private readonly IRepository<Cluster> _clusterRepository;
        private readonly IRepository<ClusterDestination> _clusterDestinationRepository;
        private readonly IRepository<ClusterHealthCheck> _clusterHealthCheckRepository;
        private readonly IRepository<ServiceGovernanceConfigure> _serviceGovernanceRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //
        private ServiceGovernanceOption _serviceGovernanceOption;
        private ConsulClient _consulClient;
        public YarpManager(IRepository<Route> routeRepository, IRepository<RouteTransform> routeTransformRepository
            , IRepository<Cluster> clusterRepository, IRepository<ClusterDestination> clusterDestinationRepository
            , IUnitOfWorkManager unitOfWorkManager, IRepository<ClusterHealthCheck> clusterHealthCheckRepository, IRepository<ServiceGovernanceConfigure> serviceGovernanceRepository)
        {
            _routeRepository = routeRepository;
            _routeTransformRepository = routeTransformRepository;
            _clusterRepository = clusterRepository;
            _clusterDestinationRepository = clusterDestinationRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _clusterHealthCheckRepository = clusterHealthCheckRepository;
            _serviceGovernanceRepository = serviceGovernanceRepository;
        }
        public async Task<YarpOption> GetConfigureAsync()
        {
            var result = new YarpOption()
            {
                Routes=new List<RouteOption>()
            };
            using var unitOfWork = _unitOfWorkManager.Begin();
            //获取所有数据
            var routes = await _routeRepository.GetListAsync(x => x.UseState);
            var routeTransforms = await _routeTransformRepository.GetListAsync();
            var clusters = await _clusterRepository.GetListAsync();
            var clusterDestinations = await _clusterDestinationRepository.GetListAsync();
            var clusterHealthChecks = await _clusterHealthCheckRepository.GetListAsync();
            //处理数据集
            RouteOption routeOption;
            Cluster cluster;
            foreach (var route in routes)
            {
                routeOption = new RouteOption()
                {
                    RouteId = route.RouteId,
                    RouteMatchPath = route.RouteMatchPath,
                    RouteName = route.RouteName,
                    RouteTransforms = routeTransforms.Where(x => x.RouteId == route.Id).Select(x => new RouteTransformOption()
                    {
                        TransformsName = x.TransformsName,
                        TransformsValue = x.TransformsValue
                    })
                    .ToList()
                };
                //集群相关数据
                cluster = clusters.Where(x => x.RouteId == route.Id).FirstOrDefault();
                routeOption.Cluster = new ClusterOption() 
                {
                    ClusterName= cluster.ClusterName,
                    LoadBalancingPolicy= cluster.LoadBalancingPolicy
                };
                if (cluster.ServiceGovernanceType == ServiceGovernanceType.Consul)
                {
                    var consulDestinations = await GetConsulServiceAsync(cluster.ServiceGovernanceName);
                    if (consulDestinations == null)
                    {
                        continue;
                    }
                    routeOption.Cluster.ClusterDestinations = consulDestinations;
                }
                else
                {
                    routeOption.Cluster.ClusterDestinations = clusterDestinations.Where(x => x.ClusterId == cluster?.Id)
                        .Select(x=>new ClusterDestinationOption() 
                        {
                            DestinationAddress = x.DestinationAddress,
                            DestinationName = x.DestinationName
                        })
                        .ToList();
                }
                routeOption.Cluster.ClusterHealthCheck = clusterHealthChecks.Where(x => x.ClusterId == cluster?.Id)
                    .Select(x => new ClusterHealthCheckOption()
                    {
                        Enabled = x.Enabled,
                        Interval = x.Interval,
                        Path = x.Path,
                        Policy = x.Policy,
                        Timeout = x.Timeout
                    })
                    .FirstOrDefault();
                //
                result.Routes.Add(routeOption);
            }
            return result;
        }
        private async Task<List<ClusterDestinationOption>> GetConsulServiceAsync(string serviceGovernanceName)
        {
            if (_serviceGovernanceOption==null)
            {
                _serviceGovernanceOption = (await _serviceGovernanceRepository.GetQueryableAsync())
                    .ProjectToType<ServiceGovernanceOption>()
                    .FirstOrDefault();
            }
            if (_serviceGovernanceOption == null)
            {
                return null;
            }
            if (_consulClient == null)
            {
                var consulClientConfiguration = new ConsulClientConfiguration();
                consulClientConfiguration.Datacenter = _serviceGovernanceOption.ConsulDatacenter;
                consulClientConfiguration.Address = new Uri(_serviceGovernanceOption.ConsulServer);
                if (!string.IsNullOrEmpty(_serviceGovernanceOption.ConsulToken) && _serviceGovernanceOption.ConsulToken != "")
                {
                    consulClientConfiguration.Token = _serviceGovernanceOption.ConsulToken;
                }
                _consulClient = new ConsulClient(consulClientConfiguration);
            }
            try
            {
                var queryResult = await _consulClient.Catalog.Service(serviceGovernanceName);
                if (queryResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Log.Error(new NotImplementedException(), $"未发现名称为{serviceGovernanceName}的服务");
                    return null;
                }
                var servcies = queryResult.Response;
                if (servcies == null || !servcies.Any())
                {
                    Log.Error(new NotImplementedException(), $"名称为{serviceGovernanceName}的服务未包含任何节点");
                    return null;
                }
                var destinations = servcies.Select(x => new ClusterDestinationOption()
                {
                    DestinationAddress = $"http://{x.ServiceAddress}:{x.ServicePort}",
                    DestinationName = Guid.NewGuid().ToString().Replace("-", "")
                })
                .ToList();
                return destinations;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Consul连接异常:{ex.Message}");
                return null;
            }
        }

       
    }
}
