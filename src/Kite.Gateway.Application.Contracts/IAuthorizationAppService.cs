using Kite.Gateway.Application.Contracts.Dtos.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    /// <summary>
    /// 授权相关接口
    /// </summary>
    public interface IAuthorizationAppService:IApplicationService
    {
        /// <summary>
        /// 获取身份认证配置信息
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseResult<SaveAuthenticationDto>> GetAuthenticationAsync();
        
        /// <summary>
        /// 保存身份认证配置信息
        /// </summary>
        /// <param name="authenticationDto"></param>
        /// <returns></returns>
        Task<HttpResponseResult> SaveAuthenticationAsync(SaveAuthenticationDto authenticationDto);
    }
}
