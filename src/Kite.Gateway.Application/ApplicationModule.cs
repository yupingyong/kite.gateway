using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp;

namespace Kite.Gateway.Application
{
    [DependsOn(
        typeof(DomainModule),
        typeof(ApplicationContractsModule)
        )]
    public class ApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //在此处注入依赖项
        }

        public override async void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var dataSeeder = context.ServiceProvider.GetService<IDataSeeder>();
            if (dataSeeder != null)
            {
                await dataSeeder.SeedAsync();
            }
        }
    }
}
