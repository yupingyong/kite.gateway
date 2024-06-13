using Kite.Gateway.Domain.Services.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Services.Auth
{
    public class AuthOptions
    {
        /// <summary>
        /// 系统默认账号
        /// </summary>
        public UserModel User { get; set; }
        /// <summary>
        /// 权限菜单
        /// </summary>
        public List<MenuModel> Menus { get; set; }
    }
}
