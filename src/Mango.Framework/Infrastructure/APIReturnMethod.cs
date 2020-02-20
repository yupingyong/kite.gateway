using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Framework.Infrastructure
{
    public class APIReturnMethod
    {
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static JsonResult ReturnSuccess(object data = null)
        {
            return new JsonResult(new { Code = 0, Message = "success", Data = data });
        }
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">提示消息</param>
        /// <returns></returns>
        public static JsonResult ReturnSuccess(string message,object data = null)
        {
            return new JsonResult(new { Code = 0, Message = message, Data = data });
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static JsonResult ReturnFailed(string message = "失败", object data = null)
        {
            return new JsonResult(new { Code = 400, Message = message, Data = data });
        }
        /// <summary>
        /// 系统代码错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static JsonResult ReturnError(string message = "系统代码错误", object data = null)
        {
            return new JsonResult(new { Code = 500, Message = message, Data = data });
        }
        /// <summary>
        /// 返回自定义状态码 
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult ReturnCustomize(int code,string message = "", object data = null)
        {
            return new JsonResult(new { Code = code, Message = message, Data = data });
        }
    }
}
