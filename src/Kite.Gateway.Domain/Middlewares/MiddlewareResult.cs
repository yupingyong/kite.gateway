using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Middlewares
{
    public class MiddlewareResult
    {
        /// <summary>
        /// Http状态码(为200时表示成功)
        /// </summary>
        public int HttpStatusCode { get; set; }
        /// <summary>
        /// 结果(希望通过返回给调用者的结果)
        /// </summary>
        public string Result { get; set; }
    }
}
