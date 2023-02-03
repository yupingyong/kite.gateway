using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Mapster;
using Yarp.ReverseProxy.Health;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Shared.Enums;

namespace Kite.Gateway.Domain.ReverseProxy
{
    internal class ClusterManager: DomainService, IClusterManager
    {
        private readonly IRepository<Cluster> _clusterRepository;
        public ClusterManager(IRepository<Cluster> clusterRepository)
        {
            _clusterRepository = clusterRepository;
        }

        public async Task<Cluster> CreateAsync(int routeId, string clusterName, ServiceGovernanceType serviceGovernanceType, string serviceGovernanceName,string loadBalancingPolicy)
        {
            var cluster = await _clusterRepository.FindAsync(x => x.RouteId == routeId);
            if (cluster != null)
            {
                throw  new Exception("同一个路由下只能创建一个集群");
            }
            return  new Cluster()
            {
                ClusterName= clusterName,
                RouteId= routeId,
                ServiceGovernanceName=string.IsNullOrEmpty(serviceGovernanceName)?"": serviceGovernanceName,
                ServiceGovernanceType= serviceGovernanceType,
                LoadBalancingPolicy= loadBalancingPolicy
            };
        }
        public Task<ClusterDestination> CreateClusterDestinationAsync(int clusterId, string destinationName, string destinationAddress)
        {
            return Task.Factory.StartNew(() => {
                return new ClusterDestination()
                {
                    DestinationAddress = destinationAddress,
                    ClusterId = clusterId,
                    DestinationName = destinationName
                };
            });
        }

        public Task<ClusterHealthCheck> CreateHealthCheckAsync<TDto>(TDto healthCheck) where TDto : class
        {
            return Task.Factory.StartNew(() => {
                var model= new ClusterHealthCheck();
                model.Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures;
                TypeAdapter.Adapt(healthCheck, model);
                return model;
            });
        }
    }
}
