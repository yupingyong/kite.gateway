using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared
{
    public class LoggerManager
    {
        public static Logger CreateLogger()
        {
            //日志输出模板
            var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            return new LoggerConfiguration()
                   .Enrich.FromLogContext()
                   .WriteTo.Logger(log =>
                   {
                       log.Filter.ByIncludingOnly(e =>
                       {
                           return e.Level == LogEventLevel.Information;
                       });
                       log.WriteTo.Console();
                       log.WriteTo.File($"data/logs/{DateTime.Now.Year}/{DateTime.Now:MM}/{DateTime.Now:dd}/information.txt", restrictedToMinimumLevel: LogEventLevel.Information
                           , outputTemplate: outputTemplate)
                           .MinimumLevel.Information()
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                   })
                  .WriteTo.Logger(log =>
                  {
                      log.Filter.ByIncludingOnly(e =>
                      {
                          return e.Level == LogEventLevel.Warning;
                      });
                      log.WriteTo.File($"data/logs/{DateTime.Now.Year}/{DateTime.Now:MM}/{DateTime.Now:dd}/warning.txt", restrictedToMinimumLevel: LogEventLevel.Warning
                          , outputTemplate: outputTemplate)
                          .MinimumLevel.Warning()
                          .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                  })
                  .WriteTo.Logger(log =>
                  {
                      log.Filter.ByIncludingOnly(e =>
                      {
                          return e.Level == LogEventLevel.Error;
                      });
                      log.WriteTo.File($"data/logs/{DateTime.Now.Year}/{DateTime.Now:MM}/{DateTime.Now:dd}/error.txt", restrictedToMinimumLevel: LogEventLevel.Error
                          , outputTemplate: outputTemplate)
                          .MinimumLevel.Error()
                          .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                  })
                  .CreateLogger();
        }
    }
}
