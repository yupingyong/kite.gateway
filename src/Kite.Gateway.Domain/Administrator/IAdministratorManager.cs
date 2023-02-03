using Kite.Gateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Administrator
{
    public interface IAdministratorManager
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="adminName">管理员账号名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        Task<Entities.Administrator> LoginAsync(string adminName,string password);
        /// <summary>
        /// 更新管理员账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adminName"></param>
        /// <returns></returns>
        Task<Entities.Administrator> UpdateAsync(int id, string adminName);
        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns></returns>
        Task<Entities.Administrator> CreateAsync(string adminName);
    }
}
