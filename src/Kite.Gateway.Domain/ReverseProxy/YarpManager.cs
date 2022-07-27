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
using Kite.Gateway.Domain.ReverseProxy.Models;
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
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //
        private ServiceGovernanceOption _serviceGovernanceOption;
        private ConsulClient _consulClient;
        public YarpManager(IRepository<Route> routeRepository, IRepository<RouteTransform> routeTransformRepository
            , IRepository<Cluster> clusterRepository, IRepository<ClusterDestination> clusterDestinationRepository
            , IUnitOfWorkManager unitOfWorkManager, IRepository<ClusterHealthCheck> clusterHealthCheckRepository
            , IOptions<ServiceGovernanceOption> options)
        {
            _routeRepository = routeRepository;
            _routeTransformRepository = routeTransformRepository;
            _clusterRepository = clusterRepository;
            _clusterDestinationRepository = clusterDestinationRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _clusterHealthCheckRepository = clusterHealthCheckRepository;
            _serviceGovernanceOption = options.Value;
        }
        public async Task<YarpOption> GetConfigureAsync()
        {
            var serviceConfigs = await GetYarpDataAsync();
            
            var routeConfigs = new List<RouteConfig>();
            var clusterConfigs = new List<ClusterConfig>();
            //处理配置项
            foreach (var cfg in serviceConfigs)
            {
                var transforms = new List<Dictionary<string, string>>();
                var transformData = new Dictionary<string, string>();
                foreach (var transform in cfg.RouteTransforms)
                {
                    transformData.Add(transform.TransformsName, transform.TransformsValue);
                }
                transforms.Add(transformData);
                //路由配置
                routeConfigs.Add(new RouteConfig()
                {
                    RouteId = cfg.Route.RouteId,
                    ClusterId = cfg.Route.RouteId,
                    Match = new RouteMatch()
                    {
                        Path = cfg.Route.RouteMatchPath
                    },
                    Transforms = transforms,
                });
                //集群配置
                //集群目的地配置数据
                var destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase);
                foreach (var item in cfg.ClusterDestinations)
                {
                    destinations.Add(item.DestinationName, new DestinationConfig()
                    {
                        Address = item.DestinationAddress
                    });
                }
                //健康检查配置数据
                HealthCheckConfig healthCheck = null;
                if (cfg.ClusterHealthCheck != null)
                {
                    healthCheck = new HealthCheckConfig()
                    {
                        Active = new ActiveHealthCheckConfig()
                        {
                            Enabled = cfg.ClusterHealthCheck.Enabled,
                            Interval = TimeSpan.FromSeconds(cfg.ClusterHealthCheck.Interval),
                            Timeout = TimeSpan.FromSeconds(cfg.ClusterHealthCheck.Timeout),
                            Path = cfg.ClusterHealthCheck.Path,
                            Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures
                        }
                    };
                }
                //
                clusterConfigs.Add(new ClusterConfig()
                {
                    ClusterId = cfg.Route.RouteId,
                    Destinations = destinations,
                    LoadBalancingPolicy = cfg.Cluster.LoadBalancingPolicy,
                    HealthCheck = healthCheck
                });
            }
            var result = new YarpOption();
            result.Routes = routeConfigs;
            result.Clusters = clusterConfigs;
            return result;
        }

        private async Task<List<YarpDataModel>> GetYarpDataAsync()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            //获取所有数据
            var routes = await _routeRepository.GetListAsync(x => x.UseState);
            var routeTransforms =await _routeTransformRepository.GetListAsync();
            var clusters =await _clusterRepository.GetListAsync();
            var clusterDestinations =await _clusterDestinationRepository.GetListAsync();
            var clusterHealthChecks = await _clusterHealthCheckRepository.GetListAsync();
            //处理数据集
            var result = new List<YarpDataModel>();
            foreach (var route in routes)
            {
                var cluster = clusters.Where(x => x.RouteId == route.Id).FirstOrDefault();
                var yarpDataModel = new YarpDataModel();
                //路由相关数据
                yarpDataModel.Route = route;
                yarpDataModel.RouteTransforms = routeTransforms.Where(x => x.RouteId == route.Id).ToList();
                //集群相关数据
                yarpDataModel.Cluster = cluster;
                if (cluster.ServiceGovernanceType == ServiceGovernanceType.Consul)
                {
                    var consulDestinations = await GetConsulServiceAsync(cluster.ServiceGovernanceName);
                    if (consulDestinations == null)
                    {
                        continue;
                    }
                    yarpDataModel.ClusterDestinations = consulDestinations;
                }
                else
                {
                    yarpDataModel.ClusterDestinations = clusterDestinations.Where(x => x.ClusterId == cluster?.Id).ToList();
                }
                yarpDataModel.ClusterHealthCheck = clusterHealthChecks.Where(x => x.ClusterId == cluster?.Id).FirstOrDefault();
                //
                result.Add(yarpDataModel);
            }
            return result;
        }
        private async Task<List<ClusterDestination>> GetConsulServiceAsync(string serviceGovernanceName)
        {
            if (!_serviceGovernanceOption.Id.HasValue)
            {
                //await _configureManager.ReloadServiceGovernance();
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
                var destinations = servcies.Select(x => new ClusterDestination()
                {
                    DestinationAddress = $"http://{x.ServiceAddress}:{x.ServicePort}",
                    ClusterId = Guid.NewGuid(),
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
