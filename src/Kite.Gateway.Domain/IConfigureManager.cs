using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Shared.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain
{
    /// <summary>
    /// 公共配置项处理服务
    /// </summary>
    public interface IConfigureManager
    {
        /// <summary>
        /// 重新加载Yarp反向代理配置数据
        /// </summary>
        /// <param name="yarpOption"></param>
        void ReloadYayp(YarpOption yarpOption);
        /// <summary>
        /// 重新加载身份认证配置信息
        /// </summary>
        /// <returns></returns>
        void ReloadAuthentication(AuthenticationOption authenticationOption);
        /// <summary>
        /// 重新加载白名单配置信息
        /// </summary>
        /// <returns></returns>
        void ReloadWhitelist(List<WhitelistOption> whitelistOptions);
        /// <summary>
        /// 重新加载中间件配置信息
        /// </summary>
        /// <returns></returns>
        void ReloadMiddleware(List<MiddlewareOption> middlewareOptions);
        /// <summary>
        /// 重新加载服务治理配置信息
        /// </summary>
        /// <returns></returns>
        void ReloadServiceGovernance(ServiceGovernanceOption serviceGovernanceOption);
    }
}
