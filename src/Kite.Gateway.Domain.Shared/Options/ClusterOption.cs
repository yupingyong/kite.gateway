using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class ClusterOption
    {
        /// <summary>
        /// 集群名称
        /// </summary>
        public string ClusterName { get; set; }
        /// <summary>
        /// 负载均衡策略
        /// FirstAlphabetical: 在不考虑负载的情况下选择按字母顺序排列的第一个可用目的地。这对于双目的地故障转移系统很有用。
        /// Random: 随机选择一个目的地。
        /// PowerOfTwoChoices (默认): 选择两个随机目标，然后选择分配请求最少的目标。这避免了它选择繁忙目的地的开销LeastRequests和最坏情况。Random
        /// RoundRobin: 通过按顺序循环选择目的地。
        /// LeastRequests: 选择分配的请求最少的目标。这需要检查所有目的地。
        /// </summary>
        public string LoadBalancingPolicy { get; set; }
        /// <summary>
        /// 集群目的地配置项
        /// </summary>
        public List<ClusterDestinationOption> ClusterDestinations { get; set; }
        /// <summary>
        /// 集群健康检查配置项
        /// </summary>
        public ClusterHealthCheckOption ClusterHealthCheck { get; set; }
    }
}
