using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Kite.Gateway.Domain.Shared.Enums;

namespace Kite.Gateway.Application.Contracts.Dtos.ReverseProxy
{
    public class CreateRouteDto
    {
        /// <summary>
        /// 路由名称
        /// </summary>
        [Required]
        public string RouteName { get; set; }

        /// <summary>
        /// 状态(0.关闭 1.开启)
        /// </summary>
        [Required]
        public bool UseState { get; set; }

        /// <summary>
        /// 路由路径规则
        /// </summary>
        [Required]
        public string RouteMatchPath { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 路由转换(移除路由路径前缀)
        /// </summary>
        public string PathRemovePrefix { get; set; }
        /// <summary>
        /// 路由转换(增加路由路径前缀)
        /// </summary>
        public string PathPrefix { get; set; }
        /// <summary>
        /// 负载均衡策略
        /// FirstAlphabetical: 在不考虑负载的情况下选择按字母顺序排列的第一个可用目的地。这对于双目的地故障转移系统很有用。
        /// Random: 随机选择一个目的地。
        /// PowerOfTwoChoices (默认): 选择两个随机目标，然后选择分配请求最少的目标。这避免了它选择繁忙目的地的开销LeastRequests和最坏情况。
        /// RoundRobin: 通过按顺序循环选择目的地。
        /// LeastRequests: 选择分配的请求最少的目标。这需要检查所有目的地。
        /// </summary>
        [Required]
        public string LoadBalancingPolicy { get; set; }
        /// <summary>
        /// 服务治理类型(0.Default  1.Consul  2.Nacos)
        /// </summary>
        [Required]
        public ServiceGovernanceType ServiceGovernanceType { get; set; }
        /// <summary>
        /// 集群目的地信息(ServiceGovernanceType!=Default 时为服务治理名称)
        /// </summary>
        [Required]
        public string ClusterDestinationValue { get; set; }
        /// <summary>
        /// 健康检查设置
        /// </summary>
        public ClusterHealthCheckDto ClusterHealthCheck { get; set; }
    }
}
