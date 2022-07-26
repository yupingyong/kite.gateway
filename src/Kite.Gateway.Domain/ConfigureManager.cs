using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Mapster;
using Kite.Gateway.Domain.Shared.Options;
using Kite.Gateway.Domain.Entities;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Kite.Gateway.Domain.Shared.Enums;
using System.Text.RegularExpressions;

namespace Kite.Gateway.Domain
{
    internal class ConfigureManager : DomainService, IConfigureManager
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly AuthenticationOption _authenticationOption;
        private readonly List<MiddlewareOption> _middlewareOptions;
        private readonly ServiceGovernanceOption _serviceGovernanceOption;
        private readonly List<WhitelistOption> _whitelistOptions;
        public ConfigureManager(IOptions<List<MiddlewareOption>> middlewareOptions
            , IOptions<ServiceGovernanceOption> serviceGovernanceOption
            , IOptions<List<WhitelistOption>> whitelistOptions
            , IOptions<TokenValidationParameters> tokenValidationParameters, AuthenticationOption authenticationOption)
        {
            _middlewareOptions = middlewareOptions.Value;
            _serviceGovernanceOption = serviceGovernanceOption.Value;
            _whitelistOptions = whitelistOptions.Value;
            _tokenValidationParameters = tokenValidationParameters.Value;
            _authenticationOption = authenticationOption;
        }

        public void ReloadAuthentication(AuthenticationOption authenticationOption)
        {
            TypeAdapter.Adapt(authenticationOption, _authenticationOption);
            //同步刷新token验证
            GetTokenValidationParameters();
        }
        private void GetTokenValidationParameters()
        {
            if (_authenticationOption.UseState)
            {
                SecurityKey securityKey;
                //判断是否启用
                if (_authenticationOption.UseSSL)
                {
                    var certificateFile = Convert.FromBase64String(_authenticationOption.CertificateFile);
                    var x509Certificate2 = new X509Certificate2(certificateFile, _authenticationOption.CertificatePassword);
                    securityKey = new X509SecurityKey(x509Certificate2);
                }
                else
                {
                    //
                    securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOption.SecurityKeyStr));
                }
                _tokenValidationParameters.ValidateIssuerSigningKey = _authenticationOption.ValidateIssuerSigningKey;//是否验证签名,不验证的画可以篡改数据，不安全
                _tokenValidationParameters.IssuerSigningKey = securityKey;//解密的密钥
                _tokenValidationParameters.ValidateIssuer = _authenticationOption.ValidateIssuer;//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
                _tokenValidationParameters.ValidIssuer = _authenticationOption.Issuer;//发行人
                _tokenValidationParameters.ValidateAudience = _authenticationOption.ValidateAudience;//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                _tokenValidationParameters.ValidAudience = _authenticationOption.Audience;//订阅人
                _tokenValidationParameters.ValidateLifetime = _authenticationOption.ValidateLifetime;//是否验证过期时间，过期了就拒绝访问
                _tokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(_authenticationOption.ClockSkew);//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                _tokenValidationParameters.RequireExpirationTime = _authenticationOption.RequireExpirationTime;
            }
        }
        public void ReloadMiddleware(List<MiddlewareOption> middlewareOptions)
        {
            //删除原有中间件配置信息
            _middlewareOptions.Clear();
            if (middlewareOptions.Any())
            {
                foreach (var middlewareOption in middlewareOptions)
                {
                    _middlewareOptions.Add(middlewareOption);
                }
            }
        }

        public void ReloadServiceGovernance(ServiceGovernanceOption serviceGovernanceOption)
        {
            TypeAdapter.Adapt(serviceGovernanceOption, _serviceGovernanceOption);
        }

        public void ReloadWhitelist(List<WhitelistOption> whitelistOptions)
        {
           
            if (whitelistOptions.Any())
            {
                _whitelistOptions.Clear();
                foreach (var whitelistOption in whitelistOptions)
                {
                    if (whitelistOption.FilterType == FilterTypeEnum.Regular)
                    {
                        whitelistOption.Regex = new Regex(whitelistOption.FilterText);
                    }
                    _whitelistOptions.Add(whitelistOption);
                }
            }
        }
    }
}
