using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Authorization
{
    public class AuthorizationComponentOptions
    {
        /// <summary>
        /// 授权数据
        /// </summary>
        public List<AuthorizationComponentModel> AuthorizationData { get; set; }
        /// <summary>
        /// 白名单过滤列表
        /// </summary>
        public List<AuthorizationComponentModel> WhitelistData { get; set; }
    }
}
