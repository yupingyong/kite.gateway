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
using Kite.Gateway.Web.Filters;
using Newtonsoft.Json;
using Volo.Abp.Json;
using Consul;
using Kite.Gateway.Domain.Authorization;
using Yarp.ReverseProxy.Configuration;
using Kite.Gateway.Domain.ReverseProxy;
using Kite.Gateway.Domain.Shared.Options;
using Kite.Gateway.Web.Middlewares;
using Kite.Gateway.Domain.Shared.Enums;
using Microsoft.Extensions.Options;
using Kite.Gateway.Application.Contracts.Dtos;
using Kite.Gateway.Application.Contracts;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;

namespace Kite.Gateway.Web
{
    [DependsOn(
         typeof(AbpAutofacModule),
         typeof(ApplicationModule),
         typeof(AbpSwashbuckleModule)
     )]
    public class WebModule:AbpModule
    {
        #region 中间件注入
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            context.Services.AddHttpClient();
            //注入会话
            context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //
            ConfigureCore(context);
            ConfigureCors(context);
            ConfigureMvc(context);
            ConfigureReverseProxy(context);
            
        }
        /// <summary>
        /// 网关核心配置项
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureCore(ServiceConfigurationContext context)
        {
            //注入网关基础配置
            context.Services.Configure<KiteGatewayOption>(context.Services.GetConfiguration().GetSection("KiteGateway"));
            //白名单配置
            context.Services.Configure<List<WhitelistOption>>(opt => { });
            //中间件配置
            context.Services.Configure<List<MiddlewareOption>>(opt => { });
            //Jwt身份认证配置
            context.Services.Configure<AuthenticationOption>(opt =>
            {
                opt.UseState = false;
            });
            context.Services.Configure<TokenValidationParameters>(opt => { });
            //Yarp反向代理配置
            context.Services.Configure<YarpOption>(opt => 
            {
                opt.Routes = new List<RouteOption>();
            });
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
            app.UseForwardedHeaders();
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
        private void LoadMainConfigureAsync(ApplicationInitializationContext context)
        {
            try
            {

                //初始化时刷新所有数据配置项
                var options = context.ServiceProvider.GetService<IOptions<KiteGatewayOption>>();
                if (options != null)
                {
                    var httpClientFactory = context.ServiceProvider.GetService<IHttpClientFactory>();
                    var httpClient = httpClientFactory.CreateClient();
                    var configureResult =  httpClient.GetFromJsonAsync<KiteResult<RefreshConfigureDto>>($"{options.Value.AdminServer}/api/kite/refresh/configure").Result;
                    if (configureResult != null && configureResult.Code == 0)
                    {
                        var refreshAppService = context.ServiceProvider.GetService<IRefreshAppService>();
                        if (refreshAppService != null)
                        {
                           var res= refreshAppService.RefreshConfigureAsync(configureResult.Data).Result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
    }
}
