using System.ComponentModel.DataAnnotations;
namespace Mango.Module.Account.Areas.Account.Models
{
    public class AccountLoginRequestModel
    {
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }
    }
}
