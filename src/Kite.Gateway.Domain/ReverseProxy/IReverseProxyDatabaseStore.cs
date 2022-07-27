using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public interface IReverseProxyDatabaseStore
    {
        /// <summary>
        /// 获取反向代理配置
        /// </summary>
        /// <returns></returns>
        InDatabaseStoreConfig GetConfig();
        /// <summary>
        /// 重新加载配置
        /// </summary>
        void Reload();
        /// <summary>
        /// 获取重新加载所需token
        /// </summary>
        /// <returns></returns>
        IChangeToken GetReloadToken();
    }
}
