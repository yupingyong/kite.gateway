using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.ReverseProxy.Models
{
    /// <summary>
    /// 服务治理配置项
    /// </summary>
    public class ServiceGovernanceModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Consul服务端地址
        /// </summary>
        public string ConsulServer { get; set; }
        /// <summary>
        /// Consul数据中心
        /// </summary>
        public string ConsulDatacenter { get; set; }
        /// <summary>
        /// Consul访问令牌
        /// </summary>
        public string ConsulToken { get; set; }
        /// <summary>
        /// Nacos服务器地址
        /// </summary>
        public string NacosServer { get; set; }
        /// <summary>
        /// Nacos群组名
        /// </summary>
        public string NacosGroupName { get; set; }
        /// <summary>
        /// Nacos命名空间ID
        /// </summary>
        public string NacosNamespaceId { get; set; }
    }
}
