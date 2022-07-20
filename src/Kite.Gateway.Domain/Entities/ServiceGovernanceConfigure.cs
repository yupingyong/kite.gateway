using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Kite.Gateway.Domain.Entities
{
    /// <summary>
    /// 服务治理配置
    /// </summary>
    public class ServiceGovernanceConfigure : Entity<Guid>
    {
        public ServiceGovernanceConfigure() { }
        public ServiceGovernanceConfigure(Guid id) : base(id)
        {

        }
        /// <summary>
        /// Consul服务端地址
        /// </summary>
        [MaxLength(512)]
        public string ConsulServer { get; set; }
        /// <summary>
        /// Consul数据中心
        /// </summary>
        [MaxLength(128)]
        public string ConsulDatacenter { get; set; }
        /// <summary>
        /// Consul访问令牌
        /// </summary>
        [MaxLength(128)]
        public string ConsulToken { get; set; }
    }
}
