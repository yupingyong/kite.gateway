using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.ReverseProxy.Models
{
    public class NacosServiceModel
    {
        // <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Clusters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CacheMillis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<NacosServiceHostModel> Hosts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long  LastRefTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Checksum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllIPs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ReachProtectionThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Valid { get; set; }
    }
}
