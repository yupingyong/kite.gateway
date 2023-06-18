
using Kite.Gateway.Admin;
using Kite.Gateway.Admin.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//��־����
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console()// ��־���������̨
          .WriteTo.File($"data/logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .CreateLogger();
builder.Host
     .UseSerilog(dispose:true)
     .UseAutofac();
//�����ļ�
builder.Services.ReplaceConfiguration(builder.Configuration);//�������ô���
//�������ע��
builder.Services.AddApplication<GatewayAdminModule>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AuthorizationServerStorage>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
