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

namespace Kite.Simple.Account.Authorization
{
    public class JwtTokenManager: IJwtTokenManager
    {
        private readonly JwtTokenOptions _options;
        private readonly IWebHostEnvironment _env;
        public JwtTokenManager(IOptions<JwtTokenOptions> options, IWebHostEnvironment env)
        {
            _options = options?.Value;
            _env = env;
        }
        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims">声明对象集合</param>
        /// <returns></returns>
        public JwtTokenResult GenerateToken(List<Claim> claims)
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
                DateTime.Now.AddSeconds(_options.ExpiresTime),
                credentials
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtTokenResult()
            {
                AccessToken = jwtToken,
                EffectiveTime = _options.ExpiresTime,
                IssueTime = DateTime.Now,
                ExpiresTime = DateTime.Now.AddSeconds(_options.ExpiresTime)
            };
        }
    }
}
