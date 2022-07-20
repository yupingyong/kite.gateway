using System.ComponentModel.DataAnnotations;

namespace Kite.Simple.Account.Models
{
    public class AccountLoginDto
    {
        /// <summary>
        /// 账号名
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
