using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Authorization
{
    public interface IAuthorizationComponent
    {
        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="areaName">区域名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">action名</param>
        /// <returns></returns>
        bool Verification(int roleId, string areaName, string controllerName, string actionName);
    }
}
