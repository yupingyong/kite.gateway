using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class RouteTransformOption
    {
        /// <summary>
        /// 交换配置项名称
        /// </summary>
        public string TransformsName { get; set; }
        /// <summary>
        /// 交换配置项值
        /// </summary>
        public string TransformsValue { get; set; }
    }
}
