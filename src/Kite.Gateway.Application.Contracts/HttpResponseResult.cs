using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kite.Gateway.Application
{
    public class HttpResponseResult<TResult>
    {
        /// <summary>
        /// 结果码(为0则表示请求成功)
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 结果数据
        /// </summary>
        public TResult Data { get; set; }

    }
    public class HttpResponseResult
    {
        /// <summary>
        /// 结果码(为0则表示请求成功)
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }
    }
}
