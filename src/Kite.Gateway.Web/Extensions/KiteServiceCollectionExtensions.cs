using Kite.Gateway.Application;
using Kite.Gateway.Domain.Shared.Infrastructure.Authorization;
using Kite.Gateway.Management.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KiteServiceCollectionExtensions
    {
        /// <summary>
        /// 注册WebApi服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKiteApi(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            
            //
            services.AddControllers(options =>
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
           
            
            return services;
        }
       
        /// <summary>
        /// 注册身份认证/授权服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKiteAuth(this IServiceCollection services) 
        {
            var configuration = services.GetConfiguration();
            services.Configure<JwtTokenOptions>(configuration.GetSection("Authorization"));
            var jwtOptions = configuration.GetSection("Authorization").Get<JwtTokenOptions>();
            services
             .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     //Token颁发机构
                     ValidIssuer = jwtOptions.Issuer,
                     //颁发给谁
                     ValidAudience = jwtOptions.Audience,
                     //这里的key要进行加密
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKeyStr)),
                     //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                     ValidateLifetime = true,
                 };
             });
            //
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.LoginPath = "/Login";
                 options.ExpireTimeSpan = new TimeSpan(2, 0, 0);
                 options.SlidingExpiration = true;

                 options.Cookie = new CookieBuilder
                 {
                     SameSite = SameSiteMode.Strict,
                     SecurePolicy = CookieSecurePolicy.Always,
                     IsEssential = true,
                     HttpOnly = true
                 };
                 options.Cookie.Name = "kite.auth";
             });
            services.AddAuthorization();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            return services;
        }
        /// <summary>
        /// 注册Swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKiteSwagger(this IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Kite.Gateway Api", Version = "v1" });
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
                    #region swagger 用 Jwt验证
                    //开启权限小锁
                    options.OperationFilter<AddResponseHeadersFilter>();
                    options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                    //在header中添加token，传递到后台
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT授权(数据将在请求头中进行传递)直接在下面框中输入Bearer {token}(注意两者之间是一个空格) \"",
                        Name = "Authorization",//jwt默认的参数名称
                        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                        Type = SecuritySchemeType.ApiKey
                    });
                    
                    #endregion
                });
            return services;
        }
    }
}
