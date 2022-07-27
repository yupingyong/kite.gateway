using Kite.Gateway.Application.Contracts.Dtos.Whitelist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    /// <summary>
    /// 白名单
    /// </summary>
    public interface IWhitelistAppService : IApplicationService
    {
        /// <summary>
        /// 获取白名单详情信息
        /// </summary>
        /// <param name="id">白名单ID</param>
        /// <returns></returns>
        Task<KiteResult<WhitelistDto>> GetAsync(Guid id);
        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <param name="id">白名单ID</param>
        /// <param name="useState">状态</param>
        /// <returns></returns>
        Task<KiteResult> UpdateUseStateAsync(Guid id,bool useState);
        /// <summary>
        /// 更新白名单数据
        /// </summary>
        /// <param name="updateWhiteList"></param>
        /// <returns></returns>
        Task<KiteResult> UpdateAsync(UpdateWhitelistDto updateWhiteList);
        /// <summary>
        /// 获取白名单列表
        /// </summary>
        /// <param name="kw"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<KitePageResult<List<WhitelistDto>>> GetListAsync(string kw = "", int page = 1, int pageSize = 10);
        /// <summary>
        /// 删除白名单
        /// </summary>
        /// <param name="id">白名单ID</param>
        /// <returns></returns>
        Task<KiteResult> DeleteAsync(Guid id);
        /// <summary>
        /// 创建白名单
        /// </summary>
        /// <param name="createWhiteList"></param>
        /// <returns></returns>
        Task<KiteResult> CreateAsync(CreateWhitelistDto createWhiteList);
    }
}
