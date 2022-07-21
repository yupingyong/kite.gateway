using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.Node
{
    public class CreateNodeDto
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [Required]
        public string NodeName { get; set; }
        /// <summary>
        /// 节点描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 节点服务端地址
        /// </summary>
        [Required]
        public string Server { get; set; }
        /// <summary>
        /// 访问Token
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
