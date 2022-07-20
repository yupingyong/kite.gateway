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

namespace Kite.Gateway.Domain
{
    internal class ConfigureManager : DomainService, IConfigureManager
    {
        private readonly AuthenticationOption _authenticationOption;
        private readonly TokenValidationParameters _tokenValidationParameters;

        private readonly List<MiddlewareOption> _middlewareOptions;
        private readonly ServiceGovernanceOption _serviceGovernanceOption;
        private readonly List<WhitelistOption> _whitelistOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ConfigureManager(IOptions<AuthenticationOption> authenticationOption, IOptions<List<MiddlewareOption>> middlewareOptions
            , IOptions<ServiceGovernanceOption> serviceGovernanceOption, IServiceProvider serviceProvider
            , IOptions<List<WhitelistOption>> whitelistOptions, IUnitOfWorkManager unitOfWorkManager
            , IOptions<TokenValidationParameters> tokenValidationParameters)
        {
            _authenticationOption = authenticationOption.Value;
            _middlewareOptions = middlewareOptions.Value;
            _serviceGovernanceOption = serviceGovernanceOption.Value;
            _serviceProvider = serviceProvider;
            _whitelistOptions = whitelistOptions.Value;
            _unitOfWorkManager = unitOfWorkManager;
            _tokenValidationParameters = tokenValidationParameters.Value;
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
                GetTokenValidationParameters();
            }
            Log.Information($"ReloadAuthenticationAsync:{JsonSerializer.Serialize(_authenticationOption)}");
        }
        private void GetTokenValidationParameters()
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
        public async Task ReloadMiddlewareAsync()
        {
            _unitOfWorkManager.Begin(true);
            var repository = _serviceProvider.GetService<IRepository<Middleware>>();
            var models = await repository.GetListAsync();
            if (models.Any())
            {
                foreach (var item in models)
                {
                    _middlewareOptions.Add(TypeAdapter.Adapt<MiddlewareOption>(item));
                }
            }
            Log.Information($"ReloadMiddlewareAsync:{JsonSerializer.Serialize(_middlewareOptions)}");
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
            Log.Information($"ReloadServiceGovernanceAsync:{JsonSerializer.Serialize(_serviceGovernanceOption)}");
        }

        public async Task ReloadWhitelistAsync()
        {
            _unitOfWorkManager.Begin(true);
            var repository = _serviceProvider.GetService<IRepository<Entities.Whitelist>>();
            var models = await repository.GetListAsync();
            if (models.Any())
            {
                foreach (var item in models)
                {
                    _whitelistOptions.Add(TypeAdapter.Adapt<WhitelistOption>(item));
                }
            }
            Log.Information($"ReloadWhitelistAsync:{JsonSerializer.Serialize(_whitelistOptions)}");
        }
    }
}
