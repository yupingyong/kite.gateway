using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application
{
    public class HttpResponseMethod
    {
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static void Failed(string message = "失败")
        {
            throw new SimpleHttpException(message);
        }
        /// <summary>
        /// 系统代码错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static void Error(string message = "系统代码错误")
        {
            throw new SimpleHttpException(message);
        }
    }
}
