using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kite.Gateway.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Volo.Abp.Json;
using Volo.Abp.AspNetCore;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;

namespace Kite.Gateway.Admin
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
         typeof(AbpAutofacModule),
         typeof(ApplicationModule),
         typeof(EntityFrameworkCoreModule)
     )]
    public class GatewayAdminModule:AbpModule
    {
        #region 中间件注入
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();
            context.Services.AddHttpClient();
            ConfigureMvc(context);
        }
        private void ConfigureMvc(ServiceConfigurationContext context)
        {
            context.Services.AddControllersWithViews();
            context.Services.AddSession();
            Configure<HubOptions>(options =>
            {
                options.DisableImplicitFromServicesParameters = true;
            });
        }
       
        
        #endregion
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            
            app.UseExceptionHandler("/Error");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
