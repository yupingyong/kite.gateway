﻿using System;
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
        /// 重新加载身份认证配置信息
        /// </summary>
        /// <returns></returns>
        Task ReloadAuthenticationAsync();
        /// <summary>
        /// 重新加载白名单配置信息
        /// </summary>
        /// <returns></returns>
        Task ReloadWhitelistAsync();
        /// <summary>
        /// 重新加载中间件配置信息
        /// </summary>
        /// <returns></returns>
        Task ReloadMiddlewareAsync();
        /// <summary>
        /// 重新加载服务治理配置信息
        /// </summary>
        /// <returns></returns>
        Task ReloadServiceGovernanceAsync();
    }
}