using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Application.Contracts.Dtos;
using Kite.Gateway.Application.Contracts.Dtos.Node;
using Kite.Gateway.Domain;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.ReverseProxy;
using Kite.Gateway.Domain.Shared.Options;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Kite.Gateway.Application
{
    public class ConfigureAppService: BaseApplicationService,IConfigureAppService
    {
        private readonly IServiceProvider _serviceProvider;
        public ConfigureAppService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            if (reloadConfigure.IsReloadWhitelist)
            {
                var repository = _serviceProvider.GetService<IRepository<Whitelist>>();
                result.Whitelists = (await repository.GetQueryableAsync())
                    .Where(x => x.UseState)
                    .Select(x => new WhitelistOption()
                    {
                        FilterText = x.FilterText,
                        Id = x.Id,
                        Name = x.Name,
                        RequestMethod = x.RequestMethod,
                        RouteId = x.RouteId.HasValue ? x.RouteId.ToString().ToLower() : "00000000-0000-0000-0000-000000000000"
                    })
                    .ToList();
            }
            if (reloadConfigure.IsReloadYarp)
            {
                var yarpManager = _serviceProvider.GetService<IYarpManager>();
                result.Yarp = await yarpManager.GetConfigureAsync();
            }
            return Ok(result);
        }

    }
}
