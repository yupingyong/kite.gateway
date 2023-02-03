using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Node
{
    public interface INodeManager
    {
        /// <summary>
        /// 更新网关节点
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="server">服务地址</param>
        /// <returns></returns>
        Task<Entities.Node> UpdateAsync(int id,string nodeName, string server);
        /// <summary>
        /// 创建网关节点
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="server">服务地址</param>
        /// <returns></returns>
        Task<Entities.Node> CreateAsync(string nodeName,string server);
    }
}
