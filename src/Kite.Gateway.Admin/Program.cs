using Kite.Gateway.Admin;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .WriteTo.File($"data/logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .CreateLogger();
builder.Host
     .UseSerilog(dispose: true)
     .UseAutofac();

builder.Services.ReplaceConfiguration(builder.Configuration);

builder.Services.AddApplication<GatewayAdminModule>();
builder.Services.AddHttpClient();

var app = builder.Build();

await app.InitializeApplicationAsync();
await app.RunAsync();
