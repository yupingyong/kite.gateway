using Microsoft.Extensions.DependencyInjection;
using Kite.Gateway.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Kite.Gateway.EntityFrameworkCore
{
    [DependsOn(
        typeof(DomainModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
        )]
    public class EntityFrameworkCoreModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlite();
            });
            context.Services.AddAbpDbContext<KiteDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            //将Entity注入到默认仓储
            var options = new AbpDbContextRegistrationOptions(typeof(KiteDbContext), context.Services);
            new EFCoreRepositoryCustomerRegister(options).AddRepositories();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var dbContext = context.ServiceProvider.GetService<KiteDbContext>();
            if (dbContext != null)
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
