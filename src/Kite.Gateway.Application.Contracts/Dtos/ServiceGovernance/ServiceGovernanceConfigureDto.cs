using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.ServiceGovernance
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceGovernanceConfigureDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Consul服务端地址
        /// </summary>
        [Required]
        public string ConsulServer { get; set; }
        /// <summary>
        /// Consul数据中心
        /// </summary>
        [Required]
        public string ConsulDatacenter { get; set; }
        /// <summary>
        /// Consul访问令牌
        /// </summary>
        public string ConsulToken { get; set; }
    }
}
