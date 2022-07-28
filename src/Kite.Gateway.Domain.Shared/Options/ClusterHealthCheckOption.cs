using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class ClusterHealthCheckOption
    {
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
        public string Policy { get; set; }
        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string Path { get; set; }
    }
}
