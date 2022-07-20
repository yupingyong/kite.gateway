using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Simple.Account.Authorization
{
    public interface IJwtTokenManager
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        JwtTokenResult GenerateToken(List<Claim> claims);
    }
}
