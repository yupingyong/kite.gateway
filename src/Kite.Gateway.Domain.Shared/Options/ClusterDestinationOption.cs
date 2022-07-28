using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    public class ClusterDestinationOption
    {
        /// <summary>
        /// 目的地名称
        /// </summary>
        public string DestinationName { get; set; }
        /// <summary>
        /// 目的地地址
        /// </summary>
        public string DestinationAddress { get; set; }
    }
}
