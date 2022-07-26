using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Kite.Gateway.Domain.ReverseProxy;
using Volo.Abp;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Domain;
using Kite.Gateway.Application.Contracts.Dtos;

namespace Kite.Gateway.Application
{
    public class RefreshAppService : BaseApplicationService, IRefreshAppService
    {
        private readonly IRefreshManager _refreshDomainService;
        private readonly IConfigureManager _configureManager;
        public RefreshAppService(IRefreshManager refreshDomainService, IConfigureManager configureManager)
        {
            _refreshDomainService = refreshDomainService;
            _configureManager = configureManager;
        }

        public async Task<HttpResponseResult> ReloadConfigureAsync(ReloadConfigureDto reloadConfigure)
        {
            //加载基础配置
            if (reloadConfigure.IsReloadAuthentication)
            {
                _configureManager.ReloadAuthentication(reloadConfigure.AuthenticationOption);
            }
            if (reloadConfigure.IsReloadWhitelist)
            {
                _configureManager.ReloadWhitelist(reloadConfigure.WhitelistOptions);
            }
            if (reloadConfigure.IsReloadServiceGovernance)
            {
                _configureManager.ReloadServiceGovernance(reloadConfigure.ServiceGovernanceOption);
            }
            if (reloadConfigure.IsReloadMiddleware)
            {
                _configureManager.ReloadMiddleware(reloadConfigure.MiddlewareOptions);
            }
            //加载路由等数据
            if (reloadConfigure.IsReloadYarp)
            {
                await _refreshDomainService.ReloadConfigAsync();
            }
            return Ok();
        }
    }
}
