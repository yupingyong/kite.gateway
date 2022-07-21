using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.ReverseProxy
{
    public class RoutePageDto
    {
        /// <summary>
        /// 路由ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 服务状态(0.关闭 1.开启)
        /// </summary>
        public bool UseState { get; set; }

        /// <summary>
        /// 路由路径规则
        /// </summary>
        public string RouteMatchPath { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        public Guid ClusterId { get; set; }
        /// <summary>
        /// 服务治理类型(0.Default  1.Consul  2.Nacos)
        /// </summary>
        public ServiceGovernanceType ServiceGovernanceType { get; set; }
        /// <summary>
        /// 服务治理名称
        /// </summary>
        public string ServiceGovernanceName { get; set; }
        /// <summary>
        /// 负载均衡策略
        /// FirstAlphabetical: 在不考虑负载的情况下选择按字母顺序排列的第一个可用目的地。这对于双目的地故障转移系统很有用。
        /// Random: 随机选择一个目的地。
        /// PowerOfTwoChoices (默认): 选择两个随机目标，然后选择分配请求最少的目标。这避免了它选择繁忙目的地的开销LeastRequests和最坏情况。
        /// RoundRobin: 通过按顺序循环选择目的地。
        /// LeastRequests: 选择分配的请求最少的目标。这需要检查所有目的地。
        /// </summary>
        public string LoadBalancingPolicy { get; set; }
    }
}
