using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Mango.Framework;
using Mango.Framework.Module;
namespace Mango.WebHost.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用每个模块独立的初始化配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomizedModuleConfigure(this IApplicationBuilder app,IWebHostEnvironment env)
        {
            var moduleInitializers = app.ApplicationServices.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, env);
            }
            return app;
        }
        /// <summary>
        /// 启用Swagger组件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                foreach (var module in GlobalConfiguration.Modules)
                {
                    if (module.IsApplicationPart)
                    {
                        c.SwaggerEndpoint($"/swagger/{module.Name}/swagger.json", $"{module.Name} API");
                    }
                }
                c.RoutePrefix = "swagger";
            });
            app.UseSwagger();
            return app;
        }
        /// <summary>
        /// 启用MVC相关组件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomizedMvc(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                foreach (var module in GlobalConfiguration.Modules)
                {
                    if (module.IsApplicationPart)
                    {
                        endpoints.MapAreaControllerRoute(
                            name: "area",
                           areaName: module.Name,
                           pattern: "{area:exists}/{controller}/{action}/{id?}"
                         );
                    }
                }
            });
            return app;
        }
    }
}
