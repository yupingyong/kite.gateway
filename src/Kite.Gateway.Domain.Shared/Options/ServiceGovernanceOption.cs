using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    /// <summary>
    /// 服务治理配置项
    /// </summary>
    public class ServiceGovernanceOption
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid? Id { get; set; }
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
    }
}
