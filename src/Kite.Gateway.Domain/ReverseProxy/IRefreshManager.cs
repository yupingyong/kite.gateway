using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public  interface IRefreshManager
    {
        /// <summary>
        /// 重载网关配置
        /// </summary>
        /// <returns></returns>
        Task ReloadConfigAsync();
    }
}
