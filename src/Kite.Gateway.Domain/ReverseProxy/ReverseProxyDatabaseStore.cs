using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Health;
using Microsoft.Extensions.Options;
using Kite.Gateway.Domain.Shared.Options;
using Volo.Abp.DependencyInjection;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public class ReverseProxyDatabaseStore : ITransientDependency, IReverseProxyDatabaseStore
    {
        private InDatabaseReloadToken _reloadToken=new InDatabaseReloadToken();
        private YarpOption _yarpOption;
        public ReverseProxyDatabaseStore(IOptions<YarpOption> options)
        {
            _yarpOption = options.Value;
        }
        public InDatabaseStoreConfig GetConfig()
        {
            //
            var proxyConfig = new InDatabaseStoreConfig(_yarpOption.Routes, _yarpOption.Clusters);
            return proxyConfig;
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
    }

        public void Reload()
        {
            Interlocked.Exchange(ref this._reloadToken,
                new InDatabaseReloadToken()).OnReload();
        }
    }
}
