using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Dtos.Auth
{
    /// <summary>
    /// 管理员账号登录
    /// </summary>
    public class AccountLoginDto
    {
        /// <summary>
        /// 管理员这账号
        /// </summary>
        [Required]
        public string AccountName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
