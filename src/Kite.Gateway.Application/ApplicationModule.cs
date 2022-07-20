using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;

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
    }
}
