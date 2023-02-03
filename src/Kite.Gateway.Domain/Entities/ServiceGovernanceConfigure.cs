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
    public class ServiceGovernanceConfigure : Entity<int>
    {
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
        /// <summary>
        /// Nacos服务器地址
        /// </summary>
        [MaxLength(512)]
        public string NacosServer { get; set; }
        /// <summary>
        /// Nacos群组名
        /// </summary>
        [MaxLength(128)]
        public string NacosGroupName { get; set; }
        /// <summary>
        /// Nacos命名空间ID
        /// </summary>
        [MaxLength(128)]
        public string NacosNamespaceId { get; set; }
    }
}
