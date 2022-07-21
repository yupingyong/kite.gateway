using Kite.Simple.Account.Authorization;

namespace Kite.Simple.Account.Models
{
    public class AccountLoginResultDto
    {
        /// <summary>
        /// 账号名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 授权token
        /// </summary>
        public JwtTokenResult JwtToken { get; set; }
    }
}
