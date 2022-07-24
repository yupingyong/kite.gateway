using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Kite.Gateway.Application;
using Kite.Gateway.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Kite.Gateway.Hosting.Filters;
using Newtonsoft.Json;
using Volo.Abp.Json;
using Consul;
using Kite.Gateway.Domain.Authorization;
using Yarp.ReverseProxy.Configuration;
using Kite.Gateway.Domain.ReverseProxy;
using Kite.Gateway.EntityFrameworkCore;
using Kite.Gateway.Domain.Shared.Options;
using Kite.Gateway.Hosting.Middlewares;
using Kite.Gateway.Domain.Shared.Enums;

namespace Kite.Gateway.Hosting
{
    [DependsOn(
         typeof(AbpAutofacModule),
         typeof(ApplicationModule),
         typeof(AbpSwashbuckleModule),
        typeof(EntityFrameworkCoreModule)
     )]
    public class HostingModule:AbpModule
    {
        #region 中间件注入
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient();
            //注入会话
            context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //
            ConfigureCore(context);
            ConfigureCors(context);
            ConfigureMvc(context);
            ConfigureReverseProxy(context);
        }
        private void ConfigureCore(ServiceConfigurationContext context)
        {
            context.Services.Configure<AuthenticationOption>(opt => 
            {
                opt.UseState = false;
            });
            context.Services.Configure<List<WhitelistOption>>(opt => { });
            context.Services.Configure<ServiceGovernanceOption>(opt => { });
            context.Services.Configure<List<MiddlewareOption>>(opt => { });
            context.Services.Configure<TokenValidationParameters>(opt => { });
        }
        /// <summary>
        /// MVC中间件注入配置
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureMvc(ServiceConfigurationContext context)
        {
            context.Services.AddControllers(options =>
            {
                // 移除 AbpValidationActionFilter
                var filterMetadata = options.Filters.FirstOrDefault(x => x is ServiceFilterAttribute attribute && attribute.ServiceType.Equals(typeof(AbpValidationActionFilter)));
                if (filterMetadata != null)
                    options.Filters.Remove(filterMetadata);
                //移除全局异常过滤器
                var errIndex = options.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
                if (errIndex > -1)
                    options.Filters.RemoveAt(errIndex);
                //
                options.Filters.Add(typeof(AbpCoreExceptionFilter));
                options.Filters.Add<KiteCoreActionFilter>();
            })
            .AddJsonOptions(opt => { });
            Configure<AbpJsonOptions>(options => options.DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 配置反向代理
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureReverseProxy(ServiceConfigurationContext context)
        {
            //注入反向代理
            context.Services.AddSingleton<IProxyConfigProvider, InDatabaseStoreConfigProvider>();
            context.Services.AddSingleton<IReverseProxyDatabaseStore, ReverseProxyDatabaseStore>();
            context.Services.AddReverseProxy();
        }
        /// <summary>
        /// 跨域注入
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureCors(ServiceConfigurationContext context)
        {
            //跨域配置
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(o => true)
                    .AllowCredentials();
                });
            });
        }
        #endregion
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
            LoadMainConfigureAsync(context);
            //
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseCors();
            app.UseRouting();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.MapReverseProxy(proxyPipeline =>
                {
                    proxyPipeline.UseMiddleware<KiteAuthorizationMiddleware>();
                    proxyPipeline.UseMiddleware<KiteExternalMiddleware>();
                });
            });
        }
        /// <summary>
        /// 加载基础配置项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async void LoadMainConfigureAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                //初始化时刷新所有数据配置项
                var configureManager = scope.ServiceProvider.GetService<IConfigureManager>();
                if (configureManager != null)
                {
                    await configureManager.ReloadAuthenticationAsync();
                    await configureManager.ReloadWhitelistAsync();
                    await configureManager.ReloadServiceGovernanceAsync();
                    await configureManager.ReloadMiddlewareAsync();
                }
            }
        }
    }
}
