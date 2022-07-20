using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Authorization
{
    public class JwtTokenValidationResult
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool Successed { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 声明信息(验证成功时返回)
        /// </summary>
        public List<ClaimModel> Claims { get; set; }
    }
}
