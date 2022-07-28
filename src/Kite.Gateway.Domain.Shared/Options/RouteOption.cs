using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class RouteOption
    {
        /// <summary>
        /// 路由ID
        /// </summary>
        public string RouteId { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 路由路径规则
        /// </summary>
        public string RouteMatchPath { get; set; }
        /// <summary>
        /// 路由转换
        /// </summary>
        public List<RouteTransformOption> RouteTransforms { get; set; }
        /// <summary>
        /// 集群配置项
        /// </summary>
        public ClusterOption Cluster { get; set; }
    }
}
