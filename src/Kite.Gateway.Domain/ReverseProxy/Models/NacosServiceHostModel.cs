using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.ReverseProxy.Models
{
    public class NacosServiceHostModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Healthy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ephemeral { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClusterName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int InstanceHeartBeatInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstanceIdGenerator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int InstanceHeartBeatTimeOut { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IpDeleteTimeout { get; set; }
    }
}
