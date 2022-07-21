using Kite.Gateway.Domain.Shared;
using Kite.Gateway.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;
using Kite.Gateway.Domain.ReverseProxy;

namespace Kite.Gateway.Domain
{
    [DependsOn(
        typeof(DomainSharedModule)
    )]
    public class DomainModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
