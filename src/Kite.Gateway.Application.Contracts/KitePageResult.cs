using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application
{
    public class KitePageResult<TResult> : KiteResult<TResult>
    {
        /// <summary>
        /// 分页获取数据时有作用
        /// </summary>
        public int Count { get; set; }
    }
}
