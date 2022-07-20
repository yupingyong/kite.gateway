using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.ReverseProxy
{
    public class ClusterDestinationDto
    {
        /// <summary>
        /// 集群目的地ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        public Guid ClusterId { get; set; }
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
