
using Kite.Gateway.Admin;
using Kite.Gateway.Admin.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//日志配置
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console()// 日志输出到控制台
          .WriteTo.File($"data/logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .CreateLogger();
builder.Host
     .UseSerilog(dispose:true)
     .UseAutofac();
//配置文件
builder.Services.ReplaceConfiguration(builder.Configuration);//修正配置错误
//基础组件注入
builder.Services.AddApplication<GatewayAdminModule>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AuthorizationServerStorage>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
