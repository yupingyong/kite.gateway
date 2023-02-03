using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.Domain.Entities
{
    /// <summary>
    /// 集群健康检查表
    /// </summary>
    public class ClusterHealthCheck : Entity<int>
    {
        /// <summary>
        /// 关联集群ID
        /// </summary>
        public int ClusterId { get; set; }
        /// <summary>
        /// 是否开启健康检查
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 检查间隔时间(秒)
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 检查策略
        /// </summary>
        [MaxLength(64)]
        public string Policy { get; set; }
        /// <summary>
        /// 健康检查地址
        /// </summary>
        [MaxLength(128)]
        public string Path { get; set; }
    }
}
