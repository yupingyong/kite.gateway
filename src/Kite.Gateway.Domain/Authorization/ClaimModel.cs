using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Authorization
{
    public class ClaimModel
    {
        /// <summary>
        /// 声明名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 声明值
        /// </summary>
        public string Value { get; set; }
    }
}
