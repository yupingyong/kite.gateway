using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Authorization
{
    public class AuthorizationComponentModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
