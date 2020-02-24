using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Mango.Framework.Module;

namespace Mango.Module.Message
{
    public class ModuleInitializer:IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSignalR();
            //初始化连接对象池
            for (int i = 1; i <= 1000; i++)
            {
                SignalR.ConnectionManager.ConnectionUsers.Add(new SignalR.ConnectionUser()
                {
                    ConnectionIds = new List<string>(),
                    UserId = string.Empty
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(options=> {
                options.MapHub<SignalR.MessageHub>("/MessageHub");
            });
        }
    }
}
