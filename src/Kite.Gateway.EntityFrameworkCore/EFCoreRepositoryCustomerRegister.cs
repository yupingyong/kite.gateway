using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Kite.Gateway.EntityFrameworkCore
{
    public class EFCoreRepositoryCustomerRegister : EfCoreRepositoryRegistrar
    {
        public EFCoreRepositoryCustomerRegister(AbpDbContextRegistrationOptions options) : base(options)
        {
        }

        public override void AddRepositories()
        {
            var compilationLibrary = DependencyContext
                    .Default
                    .CompileLibraries
                    .Where(x => !x.Serviceable && x.Type != "package" && x.Name == "Kite.Gateway.Domain")
                    .ToList();
            foreach (var _compilation in compilationLibrary)
            {
                //加载指定类
                var types = AssemblyLoadContext.Default
                .LoadFromAssemblyName(new AssemblyName(_compilation.Name))
                .GetTypes().Where(x =>
                    x.GetTypeInfo().BaseType != null
                    && x.GetBaseClasses().Any(i => i == typeof(Entity)))
                .ToList();
                types.ForEach(t =>
                {
                    RegisterDefaultRepository(t);
                });
            }
        }
    }
}
