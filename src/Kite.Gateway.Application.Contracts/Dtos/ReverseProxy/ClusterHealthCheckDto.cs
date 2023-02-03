using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.ReverseProxy
{
    public class ClusterHealthCheckDto
    {
        /// <summary>
        /// 关联集群ID
        /// </summary>
        public int ClusterId { get; set; }
        /// <summary>
        /// 是否开启健康检查
        /// </summary>
        [Required]
        public bool Enabled { get; set; }
        /// <summary>
        /// 检查间隔时间(秒)
        /// </summary>
        [Required]
        public int Interval { get; set; }
        /// <summary>
        /// 超时时间(秒)
        /// </summary>
        [Required]
        public int Timeout { get; set; }
        /// <summary>
        /// 健康检查地址
        /// </summary>
        [Required]
        public string Path { get; set; } = "/api/health";
    }
}
