using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Enums
{
    /// <summary>
    /// 服务治理类型
    /// </summary>
    public enum ServiceGovernanceType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Display(Name = "默认")]
        Default = 0,
        /// <summary>
        /// Consul
        /// </summary>
        [Display(Name = "Consul")]
        Consul = 1,
        /// <summary>
        /// Nacos
        /// </summary>
        [Display(Name = "Nacos")]
        Nacos = 2
    }
}
