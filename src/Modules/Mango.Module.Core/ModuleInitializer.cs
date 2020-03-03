using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Mango.Framework.Module;
using Mango.Framework.Data;
using Mango.Framework.Authorization;
namespace Mango.Module.Core
{
    public class ModuleInitializer:IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            var sp= serviceCollection.BuildServiceProvider();
            //获取到权限数据
            var unitOfWork = sp.GetService<IUnitOfWork<MangoDbContext>>();
            var menuRepository = unitOfWork.GetRepository<Entity.m_AccountGroupMenu>();
            var powerRepository = unitOfWork.GetRepository<Entity.m_AccountGroupPower>();
            var queryResult = powerRepository.Query()
                .Join(menuRepository.Query(), p => p.MenuId, m => m.MenuId, (p, m) => new AuthorizationComponentModel() 
                {
                    ActionName=m.ActionName,
                    AreaName=m.AreaName,
                    ControllerName=m.ControllerName,
                    RoleId=p.GroupId.Value
                })
                .ToList();
            //设置白名单数据
            List<AuthorizationComponentModel> whitelist = new List<AuthorizationComponentModel>();
            whitelist.Add(new AuthorizationComponentModel() 
            {
                ActionName="message",
                AreaName="",
                ControllerName= "home",
                RoleId=-1
            });
            whitelist.Add(new AuthorizationComponentModel()
            {
                ActionName = "index",
                AreaName = "",
                ControllerName = "home",
                RoleId = -1
            });
            whitelist.Add(new AuthorizationComponentModel()
            {
                ActionName = "error",
                AreaName = "",
                ControllerName = "home",
                RoleId = -1
            });
            //
            serviceCollection.AddSingleton(typeof(IAuthorizationComponent), new AuthorizationComponent(new AuthorizationComponentOptions() 
            {
                AuthorizationData= queryResult,
                WhitelistData= whitelist
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
        }
    }
}
