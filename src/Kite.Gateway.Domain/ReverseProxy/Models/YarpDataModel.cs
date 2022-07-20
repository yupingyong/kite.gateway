using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kite.Gateway.Domain.Entities;

namespace Kite.Gateway.Domain.ReverseProxy.Models
{
    /// <summary>
    /// Yarp反向代理数据模型
    /// </summary>
    public class YarpDataModel
    {
        /// <summary>
        /// 路由信息
        /// </summary>
        public Route Route { get; set; }
        /// <summary>
        /// 交换项配置
        /// </summary>
        public List<RouteTransform> RouteTransforms { get; set; }

        /// <summary>
        /// 集群信息
        /// </summary>
        public Cluster Cluster { get; set; }
        /// <summary>
        /// 集群健康检查
        /// </summary>
        public ClusterHealthCheck ClusterHealthCheck { get; set; }
        /// <summary>
        /// 集群目的地合集
        /// </summary>
        public List<ClusterDestination> ClusterDestinations { get; set; }
    }
}
