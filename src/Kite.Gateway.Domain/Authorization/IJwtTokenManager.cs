using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Authorization
{
    public interface IJwtTokenManager
    {
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="httpContext">请求上下文</param>
        /// <returns></returns>
        Task<JwtTokenValidationResult> ValidationTokenAsync(HttpContext httpContext);
    }
}
