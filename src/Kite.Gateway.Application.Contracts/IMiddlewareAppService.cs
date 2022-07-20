using Kite.Gateway.Application.Contracts.Dtos.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMiddlewareAppService:IApplicationService
    {
        /// <summary>
        /// 获取中间件列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        Task<HttpResponsePageResult<List<MiddlewareListDto>>> GetListAsync(string kw = "", int page = 1, int pageSize = 10);
        /// <summary>
        /// 根据ID获取中间件信息
        /// </summary>
        /// <param name="id">中间件ID</param>
        /// <returns></returns>
        Task<HttpResponseResult<MiddlewareDto>> GetAsync(Guid id);
        /// <summary>
        /// 更新中间件信息
        /// </summary>
        /// <param name="middlewareDto"></param>
        /// <returns></returns>
        Task<HttpResponseResult> UpdateAsync(UpdateMiddlewareDto middlewareDto);
        /// <summary>
        /// 更新中间件状态
        /// </summary>
        /// <param name="id">中间件ID</param>
        /// <param name="useState">状态</param>
        /// <returns></returns>
        Task<HttpResponseResult> UpdateUseStateAsync(Guid id,bool useState);
        /// <summary>
        /// 创建中间件信息
        /// </summary>
        /// <param name="middlewareDto"></param>
        /// <returns></returns>
        Task<HttpResponseResult> CreateAsync(CreateMiddlewareDto middlewareDto);
        /// <summary>
        /// 根据ID删除中间件信息
        /// </summary>
        /// <param name="id">中间件ID</param>
        /// <returns></returns>
        Task<HttpResponseResult> DeleteAsync(Guid id);
    }
}
