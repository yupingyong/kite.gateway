using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Mango.Framework;
using Mango.Framework.Data;
using Mango.Framework.Services;
using Mango.Framework.Services.Cache;
using Mango.Framework.Services.Aliyun;
using Mango.Framework.Services.Aliyun.Sms;
using Mango.Framework.Services.Tencent.Captcha;
using Mango.Framework.Services.EMail;
using Mango.Framework.Services.UPyun;
using Mango.Framework.Converter;
using Mango.Framework.Module;
using Mango.Framework.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
namespace Mango.WebHost.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly ModuleConfigurationManager _moduleConfigurationManager = new ModuleConfigurationManager();
        /// <summary>
        /// 添加服务组件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomizedServices(this IServiceCollection services, IConfiguration configuration)
        {
            //添加默认缓存组件
            services.AddMemoryCache();
            //添加Redis缓存组件
            services.AddSingleton(typeof(ICacheService), new RedisCacheService(new RedisCacheOptions()
            {
                Configuration = configuration.GetSection("Cache:ConnectionString").Value,
                InstanceName = configuration.GetSection("Cache:InstanceName").Value
            }));
            //添加阿里云组件
            services.AddSingleton(typeof(IAliyunSmsSend),new SmsSend(new AliyunOptions() { 
                AccessKeyId=configuration.GetSection("Aliyun:AccessKeyId").Value,
                AccessKeySecret= configuration.GetSection("Aliyun:AccessKeySecret").Value
            }, configuration.GetSection("Aliyun:Sms").Get<SmsOptions>()));
            //添加腾讯相关组件
            services.AddSingleton(typeof(ITencentCaptcha), new TencentCaptcha(configuration.GetSection("Tencent:Captcha").Get<CaptchaOptions>()));
            //添加邮件发送服务组件
            services.AddSingleton(typeof(IEMailService),new EMailService(new EMailOptions() { 
                FromName= configuration.GetSection("Email:FromName").Value,
                FromEMail= configuration.GetSection("Email:FromEMail").Value,
                SmtpServerUrl= configuration.GetSection("Email:SmtpServerUrl").Value,
                SmtpServerPort=Convert.ToInt32(configuration.GetSection("Email:SmtpServerPort").Value),
                SmtpAuthenticateEmail= configuration.GetSection("Email:SmtpAuthenticateEmail").Value,
                SmtpAuthenticatePasswordText= configuration.GetSection("Email:SmtpAuthenticatePasswordText").Value
            }));
            //添加又拍云文件服务组件
            services.AddSingleton(typeof(IUPyunService), new UPyunService(new UPyunOptions()
            {
                BucketName = configuration.GetSection("UPyun:BucketName").Value,
                BucketPassword = configuration.GetSection("UPyun:BucketPassword").Value,
                BucketFileUrl = configuration.GetSection("UPyun:BucketFileUrl").Value,
            }));
            //
            var sp= services.BuildServiceProvider();
            var moduleInitializers = sp.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.ConfigureServices(services);
            }
            ServiceContext.RegisterServices(services);
            return services;
        }
        /// <summary>
        /// 添加授权验证组件
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomizedAuthorization(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServer4Config.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServer4Config.GetApis())
                .AddInMemoryClients(IdentityServer4Config.GetClients());
            if (webHostEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
            return services;
        }
        /// <summary>
        /// 添加Swagger组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomizedSwagger(this IServiceCollection services, string contentRootPath)
        {
            services.AddSwaggerGen(doc =>
            {
                //添加每个模块的XML文档
                foreach (var module in GlobalConfiguration.Modules)
                {
                    if (module.IsApplicationPart)
                    {
                        doc.SwaggerDoc(module.Name, new OpenApiInfo
                        {
                            Title = $"{module.Name} API",
                            Version = module.Version,
                            Description = module.Description
                        });

                        var xmlPath = Path.Combine(contentRootPath, $@"Modules/{module.Id}/{module.Id}.xml");
                        doc.IncludeXmlComments(xmlPath);
                    }
                }
                doc.DocInclusionPredicate((docName, apiDescription) => {
                    return docName == apiDescription.ActionDescriptor.RouteValues.Where(q => q.Key == "area").FirstOrDefault().Value;
                });
                //doc.OperationFilter<AddAuthorizationTokenHeaderParameter>();
            });
            return services;
        }
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public static IServiceCollection AddModules(this IServiceCollection services, string contentRootPath)
        {
            try
            {
                GlobalConfiguration.Modules = _moduleConfigurationManager.GetModules();
                ModuleAssemblyLoadContext context = new ModuleAssemblyLoadContext();
                foreach (var module in GlobalConfiguration.Modules)
                {
                    var dllFilePath = Path.Combine(contentRootPath, $@"Modules/{module.Id}/{module.Id}.dll");
                    var moduleFolder = new DirectoryInfo(dllFilePath);
                    if (File.Exists(moduleFolder.FullName))
                    {
                        using FileStream fs = new FileStream(moduleFolder.FullName, FileMode.Open);
                        module.Assembly = context.LoadFromStream(fs);
                        //
                        RegisterModuleInitializerServices(module, ref services);
                    }
                    else
                    {
                        _logger.Warn($"{dllFilePath} file is not find!");
                    }
                    //处理视图文件程序集加载
                    var viewsFilePath = Path.Combine(contentRootPath, $@"Modules/{module.Id}/{module.Id}.Views.dll");
                    moduleFolder = new DirectoryInfo(viewsFilePath);
                    if (File.Exists(moduleFolder.FullName))
                    {
                        using FileStream viewsFileStream = new FileStream(moduleFolder.FullName, FileMode.Open);
                        module.ViewsAssembly = context.LoadFromStream(viewsFileStream);
                    }
                    else
                    {
                        _logger.Warn($"{viewsFilePath} file is not find!");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return services;
        }
        /// <summary>
        /// 添加MVC组件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
        {
            services.AddSession();

            var mvcBuilder = services.AddControllersWithViews()
                .AddJsonOptions(options=> {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
                });    
            foreach (var module in GlobalConfiguration.Modules)
            {
                if (module.IsApplicationPart)
                {
                    if (module.Assembly != null)
                        AddApplicationPart(mvcBuilder, module.Assembly);
                    if (module.ViewsAssembly != null)
                        mvcBuilder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(module.ViewsAssembly));
                }
            }
            return services;
        }
        /// <summary>
        /// 向MVC模块添加外部应用模块组件
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="assembly"></param>
        private static void AddApplicationPart(IMvcBuilder mvcBuilder, Assembly assembly)
        {
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            foreach (var part in partFactory.GetApplicationParts(assembly))
            {
                mvcBuilder.PartManager.ApplicationParts.Add(part);
            }
            
            var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, throwOnError: false);
            foreach (var relatedAssembly in relatedAssemblies)
            {
                partFactory = ApplicationPartFactory.GetApplicationPartFactory(relatedAssembly);
                foreach (var part in partFactory.GetApplicationParts(relatedAssembly))
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(part);
                    mvcBuilder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(relatedAssembly));
                }
            }
        }
        private static void RegisterModuleInitializerServices(ModuleInfo module, ref IServiceCollection services)
        {
            if (module.Assembly != null)
            {
                var moduleInitializerType = module.Assembly.GetTypes()
                        .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                {
                    services.AddSingleton(typeof(IModuleInitializer), moduleInitializerType);
                }
            }
        }
    }
}
