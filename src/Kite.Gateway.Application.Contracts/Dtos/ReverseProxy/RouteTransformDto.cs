using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.ReverseProxy
{
    public class RouteTransformDto
    {
        /// <summary>
        /// 交换配置ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 关联服务ID
        /// </summary>
        public int RouteId { get; set; }
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
