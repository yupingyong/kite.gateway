using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthRoleAuthSetDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 选择的权限菜单
        /// </summary>
        public List<Guid> MenuIds { get; set; }
    }
}
