
using Kite.Gateway.Admin;
using Kite.Gateway.Admin.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Features;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
     .ConfigureLogging((context, logBuilder) =>
     {
         Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console()// 日志输出到控制台
          .MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
          .CreateLogger();
         logBuilder.ClearProviders();
         logBuilder.AddSerilog(dispose: true);
     })
     .UseAutofac();

builder.Services.ReplaceConfiguration(builder.Configuration);//修正配置错误

builder.Services.AddApplication<GatewayAdminModule>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AuthorizationServerStorage>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
