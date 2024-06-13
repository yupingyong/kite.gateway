using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Infrastructure.Authorization
{
    public interface IJwtTokenManager
    {
        /// <summary>
        /// 校验token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<JwtTokenValidationResult> ValidationTokenAsync(string token);
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expiresTime">过期时间(秒)</param>
        /// <returns></returns>
        JwtTokenResult GenerateToken(List<Claim> claims, int expiresTime = 0);
    }
}
