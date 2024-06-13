using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace Kite.Gateway.Application.Contracts
{
    [DependsOn(
        
    )]
    public class ApplicationContractsModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //在此处注入依赖项
        }
    }
}
