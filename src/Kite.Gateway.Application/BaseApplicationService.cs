using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseApplicationService : ApplicationService
    {
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="msg">消息</param>
        /// <exception cref="Exception"></exception>
        [RemoteService(IsEnabled = false)]
        public static void ThrownFailed(string msg)
        {
            throw new SimpleHttpException(msg);
        }
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public static KiteResult Ok()
        {
            return new KiteResult()
            {
                Code = 0,
                Message = "success"
            };
        }
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public static KiteResult<TResult> Ok<TResult>(TResult data)
        {
            return new KiteResult<TResult>()
            {
                Code = 0,
                Message = "success",
                Data = data
            };
        }
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public static KitePageResult<TResult> Ok<TResult>(TResult data, int totalCount)
        {
            return new KitePageResult<TResult>()
            {
                Code = 0,
                Message = "success",
                Data = data,
                Count = totalCount
            };
        }
        /// <summary>
        /// 返回自定义状态码 
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public static KiteResult Customize(int code, string message = "")
        {
            return new KiteResult()
            {
                Code = code,
                Message = message
            };
        }
        /// <summary>
        /// 返回自定义状态码 
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public static KiteResult<TResult> Customize<TResult>(int code, string message, TResult data)
        {
            return new KiteResult<TResult>()
            {
                Code = code,
                Message = message,
                Data = data
            };
        }
    }
}
