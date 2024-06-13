using Kite.Gateway.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace DGBB.Migrations
{
    [DependsOn(
        typeof(EntityFrameworkCoreModule)
    )]
    public class MigrationsModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //在此处注入依赖项
        }
    }
}
