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
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore.Mvc;
using Kite.Gateway.Admin.Extensions;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Validation;

namespace Kite.Gateway.Admin
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
         typeof(AbpAutofacModule),
         typeof(ApplicationModule),
         typeof(EntityFrameworkCoreModule),
         typeof(AbpSwashbuckleModule)
     )]
    public class GatewayAdminModule:AbpModule
    {
        #region 中间件注入
        private ServiceConfigurationContext _context;
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _context = context;
            context.Services.AddMemoryCache();
            context.Services.AddHttpClient();
            ConfigureMvc();
            ConfigureSwaggerGen();
            ConfigureConventionalControllers();
        }
        private void ConfigureMvc()
        {
            _context.Services.AddControllersWithViews(options =>
            {
                // 移除 AbpValidationActionFilter
                var filterMetadata = options.Filters.FirstOrDefault(x => x is ServiceFilterAttribute attribute && attribute.ServiceType.Equals(typeof(AbpValidationActionFilter)));
                if (filterMetadata != null)
                {
                    options.Filters.Remove(filterMetadata);
                }
                //移除全局异常过滤器
                var errIndex = options.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
                if (errIndex > -1)
                {
                    options.Filters.RemoveAt(errIndex);
                }
                //
                options.Filters.Add(typeof(AbpCoreExceptionFilter));
                options.Filters.Add<ValidateFilter>(-1);
            });
            Configure<AbpJsonOptions>(options => options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss");
            _context.Services.AddSession();
            //如果是Blazor配置专用
            Configure<HubOptions>(options =>
            {
                options.DisableImplicitFromServicesParameters = true;
            });
        }
        /// <summary>
        /// 配置Swagger文档
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureSwaggerGen()
        {
            //在此处注入依赖项
            _context.Services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Kite.Gateway.Api", Version = "v1" });
                    var appServicesXmlPath = Path.Combine(AppContext.BaseDirectory, $"Kite.Gateway.Application.xml");
                    if (File.Exists(appServicesXmlPath))
                    {
                        options.IncludeXmlComments(appServicesXmlPath, true);
                    }
                    var appContractsServicesXmlPath = Path.Combine(AppContext.BaseDirectory, $"Kite.Gateway.Application.Contracts.xml");
                    if (File.Exists(appContractsServicesXmlPath))
                    {
                        options.IncludeXmlComments(appContractsServicesXmlPath, true);
                    }
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                    options.DocumentFilter<HiddenApiFilter>();
                    options.DocumentFilter<SwaggerEnumFilter>();
                });
        }
        /// <summary>
        /// 自动API控制器
        /// </summary>
        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "kite";
                });
            });

        }

        #endregion
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "{documentName}/swagger.json";
                });
                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "";
                    options.SwaggerEndpoint("v1/swagger.json", "Kite.Gateway.Api");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
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
