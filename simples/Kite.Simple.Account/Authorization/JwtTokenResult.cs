using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Simple.Account.Authorization
{
    public class JwtTokenResult
    {
        /// <summary>
        /// token
        /// </summary>
        public string AccessToken {  get; set; }
        /// <summary>
        /// 有效时长(单位:分钟)
        /// </summary>
        public int EffectiveTime { get; set; }
        /// <summary>
        /// 颁发时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiresTime { get; set; }
    }
}
