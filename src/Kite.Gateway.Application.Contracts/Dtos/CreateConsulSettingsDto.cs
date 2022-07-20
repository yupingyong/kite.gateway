using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos
{
    public class CreateConsulSettingsDto
    {
        /// <summary>
        /// Consul名称
        /// </summary>
        public string ConsulName { get; set; }
        /// <summary>
        /// 数据中心名称
        /// </summary>
        public string Datacenter { get; set; }
        /// <summary>
        /// 服务端地址
        /// </summary>
        public List<string> ServerUrls { get; set; }
    }
}
