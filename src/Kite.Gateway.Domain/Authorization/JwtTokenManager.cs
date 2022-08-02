using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Kite.Gateway.Domain.Shared.Options;
using Volo.Abp.Domain.Services;
using Serilog;

namespace Kite.Gateway.Domain.Authorization
{
    internal class JwtTokenManager: DomainService,IJwtTokenManager
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public JwtTokenManager(IOptions<TokenValidationParameters> tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters.Value;
        }

        public Task<JwtTokenValidationResult> ValidationTokenAsync(HttpContext httpContext)
        {
            return Task.Factory.StartNew(() => 
            {
                var jwtTokenValidationResult = new JwtTokenValidationResult()
                {
                    Successed = false,
                    Claims = new List<ClaimModel>(),
                    Message = string.Empty
                };
                if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    jwtTokenValidationResult.Successed = false;
                    jwtTokenValidationResult.Message = "401 Authorization is empty";
                    return jwtTokenValidationResult;
                }
                var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (token.Trim() == "")
                {
                    jwtTokenValidationResult.Successed = false;
                    jwtTokenValidationResult.Message = "401 Authorization token is empty";
                    return jwtTokenValidationResult;
                }
                try
                {
                    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var claimsPrincipal = jwtSecurityTokenHandler
                        .ValidateToken(token, _tokenValidationParameters, out SecurityToken securityToken);
                    //校验通过
                    if (claimsPrincipal.Claims.Any())
                    {
                        foreach (var claim in claimsPrincipal.Claims)
                        {
                            jwtTokenValidationResult.Claims.Add(new ClaimModel() 
                            {
                                Name=claim.Type,
                                Value=claim.Value
                            });
                        }
                    }
                    jwtTokenValidationResult.Successed = true;
                    return jwtTokenValidationResult;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    jwtTokenValidationResult.Successed = false;
                    jwtTokenValidationResult.Message = $"401 Unauthorized";
                    return jwtTokenValidationResult;
                }
            });
        }
    }
}
