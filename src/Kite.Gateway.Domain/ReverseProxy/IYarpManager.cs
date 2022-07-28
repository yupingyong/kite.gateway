using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kite.Gateway.Domain.Shared.Options;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public interface IYarpManager
    {
        /// <summary>
        /// 获取Yarp配置数据
        /// </summary>
        /// <returns></returns>
        Task<YarpOption> GetConfigureAsync();
    }
}
