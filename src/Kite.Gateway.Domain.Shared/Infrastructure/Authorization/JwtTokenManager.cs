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
using Serilog;
using Volo.Abp.DependencyInjection;

namespace Kite.Gateway.Domain.Shared.Infrastructure.Authorization
{
    internal class JwtTokenManager: ITransientDependency, IJwtTokenManager
    {
        private readonly JwtTokenOptions _options;
        private readonly IWebHostEnvironment _env;
        public JwtTokenManager(IOptions<JwtTokenOptions> options, IWebHostEnvironment env)
        {
            _options = options?.Value;
            _env = env;
        }
        public Task<JwtTokenValidationResult> ValidationTokenAsync(string token)
        {
            return Task.Factory.StartNew(() =>
            {
                var jwtTokenValidationResult = new JwtTokenValidationResult()
                {
                    Successed = false,
                    Claims = new List<ClaimModel>(),
                    Message = string.Empty
                };
                try
                {
                    var tokenValidationParameters = GetTokenValidationParameters();
                    //
                    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var claimsPrincipal = jwtSecurityTokenHandler
                        .ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                    //校验通过
                    if (claimsPrincipal.Claims.Any())
                    {
                        foreach (var claim in claimsPrincipal.Claims)
                        {
                            jwtTokenValidationResult.Claims.Add(new ClaimModel()
                            {
                                Name = claim.Type,
                                Value = claim.Value
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
        private TokenValidationParameters GetTokenValidationParameters()
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                //Token颁发机构
                ValidIssuer = _options.Issuer,
                //颁发给谁
                ValidAudience = _options.Audience,
                //这里的key要进行加密
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKeyStr)),
                //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                ValidateLifetime = true,
            };
            return tokenValidationParameters;
        }
    
        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims">声明对象集合</param>
        /// <param name="expiresTime">过期时间</param>
        /// <returns></returns>
        public JwtTokenResult GenerateToken(List<Claim> claims,int expiresTime=0)
        {
            SigningCredentials credentials;
            //判断是否启用
            if (_options.UseSSL)
            {
                var path = Path.Combine(_env.WebRootPath, _options.CertificatePath);
                if (!File.Exists(path))
                {
                    throw new ArgumentNullException($"证书文件在[{path}]下不存在");
                }
                var x509Certificate2 = new X509Certificate2(path, _options.CertificatePassword);
                credentials = new SigningCredentials(new X509SecurityKey(x509Certificate2), SecurityAlgorithms.RsaSha256);
            }
            else
            {
                //
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKeyStr));
                credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            }
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddSeconds(expiresTime > 0 ? expiresTime : _options.ExpiresTime),
                credentials
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtTokenResult()
            {
                AccessToken = jwtToken,
                EffectiveTime = expiresTime > 0 ? expiresTime : _options.ExpiresTime,
                IssueTime = DateTime.Now,
                ExpiresTime = DateTime.Now.AddSeconds(expiresTime > 0 ? expiresTime : _options.ExpiresTime)
            };
        }
    }
}
