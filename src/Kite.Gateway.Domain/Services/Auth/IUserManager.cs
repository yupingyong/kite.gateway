using Kite.Gateway.Domain.Services.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Services.Auth
{
    public interface IUserManager
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<LoginResultModel> LoginAsync(string userName,string password);
    }
}
