using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Authorization
{
    public class AuthorizationComponent: IAuthorizationComponent
    {
        private AuthorizationComponentOptions _options;
        public AuthorizationComponent(AuthorizationComponentOptions options)
        {
            _options = options;
        }
        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="areaName">区域名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">action名</param>
        /// <returns></returns>
        public bool Verification(int roleId, string areaName, string controllerName, string actionName)
        {
            bool Result = false;
            if (WhitelistFilter(areaName, controllerName, actionName))
                return true;

            //验证是否有该访问权限
            var list = _options.AuthorizationData.Where(q=>q.RoleId== roleId).ToList();
            if (list != null)
            {
                //开始权限验证
                foreach (var m in list)
                {
                    if ( m.AreaName.ToLower() == areaName && m.ControllerName.ToLower() == controllerName && m.ActionName.ToLower() == actionName)
                    {
                        //当匹配到权限时返回正确结果并且跳出循环
                        Result = true;
                        break;
                    }
                }
            }
            return Result;
        }
        /// <summary>
        /// 过滤白名单
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private bool WhitelistFilter(string areaName, string controllerName, string actionName)
        {
            bool result = false;
            if (_options.WhitelistData != null)
            {
                foreach (var wd in _options.WhitelistData)
                {
                    if (wd.AreaName.ToLower() == areaName && wd.ControllerName.ToLower() == controllerName && wd.ActionName.ToLower() == actionName)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
