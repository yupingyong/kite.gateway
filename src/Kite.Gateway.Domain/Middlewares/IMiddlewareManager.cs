using Kite.Gateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Middlewares
{
    public interface IMiddlewareManager
    {
        /// <summary>
        /// 获取所有中间件配置信息
        /// </summary>
        /// <returns></returns>
        Task<List<Middleware>> GetListAsync();
        /// <summary>
        /// 更新中间件信息
        /// </summary>
        /// <param name="id">中间件ID</param>
        /// <param name="name">中间件名称</param>
        /// <param name="server">服务端地址</param>
        /// <returns></returns>
        Task<Middleware> UpdateAsync(Guid id,string name, string server);
        /// <summary>
        /// 创建中间件
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="server">服务端地址</param>
        /// <returns></returns>
        Task<Middleware> CreateAsync(string name,string server);
    }
}
