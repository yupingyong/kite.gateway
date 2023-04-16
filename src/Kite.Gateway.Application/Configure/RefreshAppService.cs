using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;
using Kite.Gateway.Domain.ReverseProxy;
using Volo.Abp;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Domain;
using Kite.Gateway.Application.Contracts.Dtos.Node;
using Kite.Gateway.Application.Contracts.Dtos;
using Mapster;
using Kite.Gateway.Domain.Shared.Options;
using Kite.Gateway.Domain.Entities;

namespace Kite.Gateway.Application.Configure
{
    public class RefreshAppService : BaseApplicationService, IRefreshAppService
    {
        private readonly IRefreshManager _refreshManager;
        private readonly IConfigureManager _configureManager;
        public RefreshAppService(IConfigureManager configureManager, IRefreshManager refreshManager)
        {
            _configureManager = configureManager;
            _refreshManager = refreshManager;
        }
        public async Task<KiteResult> RefreshConfigureAsync(RefreshConfigureDto refreshConfigure)
        {
            //加载基础配置
            if (refreshConfigure.Authentication != null)
            {
                _configureManager.ReloadAuthentication(refreshConfigure.Authentication);
            }
            if (refreshConfigure.Whitelists != null)
            {
                _configureManager.ReloadWhitelist(refreshConfigure.Whitelists);
            }
            if (refreshConfigure.Middlewares != null)
            {
                _configureManager.ReloadMiddleware(refreshConfigure.Middlewares);
            }
            //加载路由等数据
            if (refreshConfigure.Yarp != null)
            {
                _configureManager.ReloadYayp(refreshConfigure.Yarp);
                await _refreshManager.ReloadConfigAsync();
            }
            return Ok();
        }
    }
}
