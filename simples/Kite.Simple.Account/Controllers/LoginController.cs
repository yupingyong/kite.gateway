using Kite.Simple.Account.Authorization;
using Kite.Simple.Account.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Kite.Simple.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtTokenManager _jwtTokenManager;

        public LoginController(IJwtTokenManager jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        /// <summary>
        /// 登录(测试账号:test/123456)
        /// </summary>
        /// <param name="accountLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostAsync(AccountLoginDto accountLogin)
        {
            if (accountLogin.AccountName != "test" && accountLogin.Password != "123456")
            {
                throw new Exception("账号密码错误");
            }
            var result = new AccountLoginResultDto();
            result.AccountName = accountLogin.AccountName;
            result.NickName = "法外狂徒张三";
            //
            var claims = new List<Claim>();
            claims.Add(new Claim("AccountName", accountLogin.AccountName));
            claims.Add(new Claim("NickName", result.NickName));
            result.JwtToken = _jwtTokenManager.GenerateToken(claims);
            return Ok(result);
        }
    }
}
