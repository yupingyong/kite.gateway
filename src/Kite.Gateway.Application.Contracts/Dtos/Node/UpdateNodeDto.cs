using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Kite.Gateway.Application.Contracts.Dtos.Node
{
    public class UpdateNodeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        [Required]
        public string NodeName { get; set; }
        /// <summary>
        /// 节点描述
        /// </summary>
        [Required]
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
        public string AccessToken { get; set; }
    }
}
