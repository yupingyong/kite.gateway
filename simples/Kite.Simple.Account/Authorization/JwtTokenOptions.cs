using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Simple.Account.Authorization
{
    public class JwtTokenOptions
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 观察者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// token生成与验证密钥
        /// </summary>
        public string SecurityKeyStr { get; set; }
        /// <summary>
        /// 过期时间(单位:秒)
        /// </summary>
        public int ExpiresTime { get; set; }
    }
}
