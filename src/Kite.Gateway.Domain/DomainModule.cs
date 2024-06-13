using Kite.Gateway.Domain.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Kite.Gateway.Domain
{
    [DependsOn(
        typeof(DomainSharedModule)
    )]
    public class DomainModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration =context.Services.GetConfiguration();
            //注册默认配置项
        }
        
    }
}
