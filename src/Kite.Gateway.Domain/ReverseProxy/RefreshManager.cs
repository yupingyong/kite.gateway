using Kite.Gateway.Domain.ReverseProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Kite.Gateway.Domain.ReverseProxy
{
    internal class RefreshManager:DomainService, IRefreshManager
    {
        private readonly IReverseProxyDatabaseStore _reverseProxyMemoryStore;
        public RefreshManager(IReverseProxyDatabaseStore reverseProxyMemoryStore)
        {
            _reverseProxyMemoryStore = reverseProxyMemoryStore;
        }
        public Task ReloadConfigAsync()
        {
            return Task.Factory.StartNew(() => 
            {
                _reverseProxyMemoryStore.Reload();
            });
        }
    }
}
