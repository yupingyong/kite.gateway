using Kite.Gateway.Application.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    public interface IPluginAppService : IApplicationService
    {
        /// <summary>
        /// 获取中间件列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        Task<HttpResponseResult<List<PluginDto>>> GetListAsync(string kw = "", int page = 1, int pageSize = 10);
        /// <summary>
        /// 根据ID获取中间件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HttpResponseResult<PluginDto>> GetAsync(Guid id);
        /// <summary>
        /// 删除中间件
        /// </summary>
        /// <param name="id">中间件ID</param>
        /// <returns></returns>
        Task<HttpResponseResult> DeleteAsync(Guid id);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">中间件ID</param>
        /// <param name="state">状态(1.启用 0.关闭)</param>
        /// <returns></returns>
        Task<HttpResponseResult> UpdateStateAsync(Guid id, int state);
        /// <summary>
        /// 更新中间件信息
        /// </summary>
        /// <param name="updateMiddleware"></param>
        /// <returns></returns>
        Task<HttpResponseResult> UpdateAsync(UpdatePluginDto updateMiddleware);
    }
}
