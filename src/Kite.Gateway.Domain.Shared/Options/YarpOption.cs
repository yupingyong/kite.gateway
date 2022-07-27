using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class YarpOption
    {
        /// <summary>
        /// 路由配置数据
        /// </summary>
        public List<RouteConfig> Routes { get; set; } 
        /// <summary>
        /// 集群配置数据
        /// </summary>
        public List<ClusterConfig> Clusters { get; set; }
    }
}
