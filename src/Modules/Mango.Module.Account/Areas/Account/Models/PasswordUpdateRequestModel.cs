using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Account.Areas.Account.Models
{
    public class PasswordUpdateRequestModel
    {
        /// <summary>
        /// 账户ID
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 原密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }
}
