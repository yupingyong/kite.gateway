using Kite.Gateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Authorization
{
    public interface IAuthenticationManager
    {
        /// <summary>
        /// 获取身份认证配置项信息
        /// </summary>
        /// <returns></returns>
        Task<AuthenticationConfigure> GetConfigureAsync();
    }
}
