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
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ConfigureManager(IOptions<List<MiddlewareOption>> middlewareOptions
            , IOptions<ServiceGovernanceOption> serviceGovernanceOption, IServiceProvider serviceProvider
            , IOptions<List<WhitelistOption>> whitelistOptions, IUnitOfWorkManager unitOfWorkManager
            , IOptions<TokenValidationParameters> tokenValidationParameters, AuthenticationOption authenticationOption)
        {
            _middlewareOptions = middlewareOptions.Value;
            _serviceGovernanceOption = serviceGovernanceOption.Value;
            _serviceProvider = serviceProvider;
            _whitelistOptions = whitelistOptions.Value;
            _unitOfWorkManager = unitOfWorkManager;
            _tokenValidationParameters = tokenValidationParameters.Value;
            _authenticationOption = authenticationOption;
        }

        public async Task ReloadAuthenticationAsync()
        {
            _unitOfWorkManager.Begin(true);
            var repository= _serviceProvider.GetService<IRepository<AuthenticationConfigure>>();
            var model =await repository.FirstOrDefaultAsync();
            if (model != null)
            {
                TypeAdapter.Adapt(model, _authenticationOption);
                //同步刷新token验证
                GetTokenValidationParameters(model);
            }
            
        }
        private void GetTokenValidationParameters(AuthenticationConfigure authenticationConfigure)
        {
            if (authenticationConfigure.UseState)
            {
                SecurityKey securityKey;
                //判断是否启用
                if (authenticationConfigure.UseSSL)
                {
                    var certificateFile = Convert.FromBase64String(authenticationConfigure.CertificateFile);
                    var x509Certificate2 = new X509Certificate2(certificateFile, authenticationConfigure.CertificatePassword);
                    securityKey = new X509SecurityKey(x509Certificate2);
                }
                else
                {
                    //
                    securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfigure.SecurityKeyStr));
                }
                _tokenValidationParameters.ValidateIssuerSigningKey = authenticationConfigure.ValidateIssuerSigningKey;//是否验证签名,不验证的画可以篡改数据，不安全
                _tokenValidationParameters.IssuerSigningKey = securityKey;//解密的密钥
                _tokenValidationParameters.ValidateIssuer = authenticationConfigure.ValidateIssuer;//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
                _tokenValidationParameters.ValidIssuer = authenticationConfigure.Issuer;//发行人
                _tokenValidationParameters.ValidateAudience = authenticationConfigure.ValidateAudience;//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                _tokenValidationParameters.ValidAudience = authenticationConfigure.Audience;//订阅人
                _tokenValidationParameters.ValidateLifetime = authenticationConfigure.ValidateLifetime;//是否验证过期时间，过期了就拒绝访问
                _tokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(authenticationConfigure.ClockSkew);//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                _tokenValidationParameters.RequireExpirationTime = authenticationConfigure.RequireExpirationTime;
            }
        }
        public async Task ReloadMiddlewareAsync()
        {
            _unitOfWorkManager.Begin(true);
            var repository = _serviceProvider.GetService<IRepository<Middleware>>();
            var models = (await repository.GetQueryableAsync())
                .Where(x=>x.UseState)
                .OrderByDescending(x=>x.ExecWeight)
                .ProjectToType<MiddlewareOption>()
                .ToList();
            //删除原有中间件配置信息
            _middlewareOptions.Clear();
            if (models.Any())
            {
                foreach (var item in models)
                {
                    _middlewareOptions.Add(item);
                }
            }
        }

        public async Task ReloadServiceGovernanceAsync()
        {
            _unitOfWorkManager.Begin(true);
            var repository = _serviceProvider.GetService<IRepository<ServiceGovernanceConfigure>>();
            var model = await repository.FirstOrDefaultAsync();
            if (model != null)
            {
                TypeAdapter.Adapt(model, _serviceGovernanceOption);
            }
        }

        public async Task ReloadWhitelistAsync()
        {
            _unitOfWorkManager.Begin(true);
            var repository = _serviceProvider.GetService<IRepository<Entities.Whitelist>>();
            var models = (await repository.GetQueryableAsync())
                .Where(x=>x.UseState)
                .ProjectToType<WhitelistOption>()
                .ToList();
            if (models.Any())
            {
                foreach (var item in models)
                {
                    if (item.FilterType == FilterTypeEnum.Regular)
                    {
                        item.Regex = new Regex(item.FilterText);
                    }
                    _whitelistOptions.Add(item);
                }
            }
        }
    }
}
