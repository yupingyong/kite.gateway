using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Middlewares
{
    public interface IMiddlewareContext
    {
        /// <summary>
        /// 执行中间件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<MiddlewareResult> InvokeMiddlewareAsync();
    }
}
