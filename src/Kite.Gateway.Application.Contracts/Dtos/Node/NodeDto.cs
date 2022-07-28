using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.Node
{
    public class NodeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 节点描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 节点服务端地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}
