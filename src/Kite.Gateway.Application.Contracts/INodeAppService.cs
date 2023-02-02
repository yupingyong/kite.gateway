using Kite.Gateway.Application.Contracts.Dtos.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    public interface INodeAppService:IApplicationService
    {
        /// <summary>
        /// 获取所有节点数据
        /// </summary>
        /// <returns></returns>
        Task<KiteResult<List<NodeDto>>> GetAsync();
        /// <summary>
        /// 获取节点详情信息
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <returns></returns>
        Task<KiteResult<NodeDto>> GetAsync(Guid id);
        /// <summary>
        /// 更新节点数据
        /// </summary>
        /// <param name="updateNode"></param>
        /// <returns></returns>
        Task<KiteResult> UpdateAsync(UpdateNodeDto updateNode);
        /// <summary>
        /// 获取节点列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        Task<KitePageResult<List<NodeDto>>> GetNodesAsync(int page = 1, int pageSize = 10);
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <returns></returns>
        Task<KiteResult> DeleteAsync(Guid id);
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="createNode"></param>
        /// <returns></returns>
        Task<KiteResult> CreateAsync(CreateNodeDto createNode);
    }
}
