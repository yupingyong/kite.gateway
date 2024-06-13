using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Services.Auth.Models
{
    public class LoginResultModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserModel User { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public List<MenuModel> Menus { get; set; }
    }
}
