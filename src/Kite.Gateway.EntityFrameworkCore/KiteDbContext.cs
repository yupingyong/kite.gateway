using Kite.Gateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Serilog;
using System.Reflection;
using System.Runtime.Loader;
using System;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class KiteDbContext : AbpDbContext<KiteDbContext>
    {
        public KiteDbContext(DbContextOptions<KiteDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            try
            {
                //
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
                        builder.Model.AddEntityType(t);
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            base.OnModelCreating(builder);
        }
    }
}
