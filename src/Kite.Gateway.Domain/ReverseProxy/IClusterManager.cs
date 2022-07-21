using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public interface IClusterManager
    {
        /// <summary>
        /// 创建健康检查
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="healthCheck"></param>
        /// <returns></returns>
        Task<ClusterHealthCheck> CreateHealthCheckAsync<TDto>(TDto healthCheck) where TDto : class;
        /// <summary>
        /// 创建集群 
        /// </summary>
        /// <param name="routeId">路由ID</param>
        /// <param name="clusterName">集群名称</param>
        /// <param name="serviceGovernanceType">服务治理类型</param>
        /// <param name="serviceGovernanceName">服务名称</param>
        /// <param name="loadBalancingPolicy">负载均衡策略</param>
        /// <returns></returns>
        Task<Cluster> CreateAsync(Guid routeId,string clusterName, ServiceGovernanceType serviceGovernanceType,string serviceGovernanceName, string loadBalancingPolicy);
        /// <summary>
        /// 创建集群目的地
        /// </summary>
        /// <param name="clusterId">所属集群ID</param>
        /// <param name="destinationName">目的地名称</param>
        /// <param name="destinationAddress">目的地地址</param>
        /// <returns></returns>
        Task<ClusterDestination> CreateClusterDestinationAsync(Guid clusterId,string destinationName,string destinationAddress);
    }
}
