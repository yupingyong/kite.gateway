using Kite.Gateway.Admin;
using Serilog.Events;
using Serilog;
using Kite.Gateway.Domain.Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
Log.Logger = LoggerManager.CreateLogger();
builder.Host
     .UseSerilog(dispose: true)
     .UseAutofac();

builder.Services.ReplaceConfiguration(builder.Configuration);

builder.Services.AddApplication<GatewayAdminModule>();
builder.Services.AddHttpClient();

var app = builder.Build();

await app.InitializeApplicationAsync();
await app.RunAsync();
