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

namespace Kite.Gateway.Application
{
    public class RefreshAppService : BaseApplicationService, IRefreshAppService
    {
        private readonly IRefreshManager _refreshManager;
        private readonly IConfigureManager _configureManager;
        private readonly IServiceProvider _serviceProvider;
        public RefreshAppService( IConfigureManager configureManager, IServiceProvider serviceProvider, IRefreshManager refreshManager)
        {
            _configureManager = configureManager;
            _serviceProvider = serviceProvider;
            _refreshManager = refreshManager;
        }

        public async Task<KiteResult<RefreshConfigureDto>> GetConfigureAsync(ReloadConfigureDto reloadConfigure)
        {
            var result = new RefreshConfigureDto();
            if (reloadConfigure.IsReloadAuthentication)
            {
                var repository = _serviceProvider.GetService<IRepository<AuthenticationConfigure>>();
                result.Authentication = (await repository.GetQueryableAsync())
                    .ProjectToType<AuthenticationOption>()
                    .FirstOrDefault();
            }
            if (reloadConfigure.IsReloadMiddleware)
            {
                var repository = _serviceProvider.GetService<IRepository<Middleware>>();
                result.Middlewares = (await repository.GetQueryableAsync())
                    .Where(x => x.UseState)
                    .OrderByDescending(x => x.ExecWeight)
                    .ProjectToType<MiddlewareOption>()
                    .ToList();
            }
            if (reloadConfigure.IsReloadServiceGovernance)
            {
                var repository = _serviceProvider.GetService<IRepository<ServiceGovernanceConfigure>>();
                result.ServiceGovernance = (await repository.GetQueryableAsync())
                    .ProjectToType<ServiceGovernanceOption>()
                    .FirstOrDefault();
            }
            if (reloadConfigure.IsReloadWhitelist)
            {
                var repository = _serviceProvider.GetService<IRepository<Whitelist>>();
                result.Whitelists = (await repository.GetQueryableAsync())
                    .Where(x => x.UseState)
                    .ProjectToType<WhitelistOption>()
                    .ToList();
            }
            if (reloadConfigure.IsReloadYarp)
            {
                var yarpManager = _serviceProvider.GetService<IYarpManager>();
                result.Yarp = await yarpManager.GetConfigureAsync();
            }
            return Ok(result);
        }

        public async Task<KiteResult> RefreshConfigureAsync(RefreshConfigureDto refreshConfigure)
        {
            //加载基础配置
            if (refreshConfigure.Authentication != null)
            {
                _configureManager.ReloadAuthentication(refreshConfigure.Authentication);
            }
            if (refreshConfigure.Whitelists!=null)
            {
                _configureManager.ReloadWhitelist(refreshConfigure.Whitelists);
            }
            if (refreshConfigure.ServiceGovernance!=null)
            {
                _configureManager.ReloadServiceGovernance(refreshConfigure.ServiceGovernance);
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
