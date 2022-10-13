using Kite.Gateway.Web;
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
     .UseSerilog(dispose: true)
     .UseAutofac();


//配置请求体大小
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = int.MaxValue;
});
builder.Services.ReplaceConfiguration(builder.Configuration);//修正配置错误

builder.Services.AddApplication<WebModule>();
var app = builder.Build();
app.InitializeApplication();
app.MapGet("/", context => context.Response.WriteAsync("kite gateway run success!"));
app.Run();
