using Autofac.Core;
using DGBB.Migrations;
using Kite.Gateway.Application;
using Kite.Gateway.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Kite.Gateway.Web
{
    [DependsOn(
         typeof(AbpAutofacModule),
         typeof(ApplicationModule),
        typeof(AbpSwashbuckleModule),
        typeof(MigrationsModule)
     )]
    public class WebModule : AbpModule
    {
        private ServiceConfigurationContext _context;
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _context = context;
            ConfigureCore();
            ConfigureRazorPage();
            ConfigureAbpApi();
            _context.Services.AddKiteApi();
            _context.Services.AddKiteAuth();
            _context.Services.AddKiteSwagger();
        }
        private void ConfigureCore()
        {
            var configuration = _context.Services.GetConfiguration();
            _context.Services.AddMemoryCache();
            _context.Services.AddHttpClient();
            _context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        private void ConfigureRazorPage()
        {
            var configuration = _context.Services.GetConfiguration();
            _context.Services.AddRazorPages(options =>
            {
                //设置授权访问
                options.Conventions.AllowAnonymousToPage("/Login");
                options.Conventions.AllowAnonymousToPage("/Error");
                options.Conventions.AuthorizePage("/Index");
                options.Conventions.AuthorizePage("/Home");
                options.Conventions.AuthorizeFolder("/Auth");
            });
            
        }
        /// <summary>
        /// Abp接口服务配置
        /// </summary>
        private void ConfigureAbpApi() 
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "kite";
                    opts.UrlControllerNameNormalizer = (controller) =>
                    {
                        return GetControllerRoute(controller.ControllerName);
                    };
                });
            });
            Configure<AbpJsonOptions>(options =>
            {
                options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                options.InputDateTimeFormats.Add("yyyy-MM-dd HH:mm:ss");
            });
        }
        /// <summary>
        /// 处理路由
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        private string GetControllerRoute(string controllerName)
        {
            var chars = new List<string>();
            var str = new List<string>();
            foreach (char c in controllerName)
            {
                if (Char.IsUpper(c))
                {
                    if (chars.Any())
                    {
                        str.Add(string.Join("", chars));
                        chars.Clear();
                    }
                }
                chars.Add(c.ToString().ToLower());
            }
            str.Add(string.Join("", chars));
            if (str.Count > 1)
            {
                return string.Join("/", str);
            }
            return controllerName.ToLower();
        }
        /// <summary>
        /// 应用服务初始化
        /// </summary>
        /// <param name="context"></param>
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseCors();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
