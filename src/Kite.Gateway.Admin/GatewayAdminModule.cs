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
using Kite.Gateway.Admin.Core;
using Volo.Abp.AspNetCore;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.EntityFrameworkCore;
using BootstrapBlazor.Components;
using Microsoft.Extensions.Options;

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
            ConfigureBlazor(context);
            ConfigureMvc(context);
        }
        private void ConfigureMvc(ServiceConfigurationContext context)
        {
            context.Services.AddControllers();
        }
        /// <summary>
        /// Blazor
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureBlazor(ServiceConfigurationContext context)
        {
            context.Services.AddRazorPages();
            context.Services.AddServerSideBlazor()
                .AddHubOptions(options =>
                {
                    options.MaximumReceiveMessageSize = int.MaxValue;
                });
            context.Services.AddBootstrapBlazor(options =>
            {
                // 统一设置 Toast 组件自动消失时间
                options.ToastDelay = 4000;
                options.ToastPlacement = Placement.TopEnd;
                options.SupportedCultures = new List<string> { "zh" };

                options.FallbackCulture = "zh";
                options.DefaultCultureInfo = "zh";
            });
            // 增加多语言支持配置信息
            context.Services.AddRequestLocalization<IOptionsMonitor<BootstrapBlazorOptions>>((localizerOption, blazorOption) =>
            {
                blazorOption.OnChange(op => Invoke(op));
                Invoke(blazorOption.CurrentValue);

                void Invoke(BootstrapBlazorOptions option)
                {
                    var supportedCultures = option.GetSupportedCultures();
                    localizerOption.SupportedCultures = supportedCultures;
                    localizerOption.SupportedUICultures = supportedCultures;
                }
            });
        }
        
        #endregion
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            var option = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            if (option != null)
            {
                app.UseRequestLocalization(option.Value);
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseExceptionHandler("/Error");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
