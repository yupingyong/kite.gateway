using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class KiteGatewayOption
    {
        /// <summary>
        /// 网关管理端服务器地址
        /// </summary>
        public string AdminServer { get; set; }
        /// <summary>
        /// 配置数据热更新访问授权token
        /// </summary>
        public string AccessToken { get; set; }
    }
}
