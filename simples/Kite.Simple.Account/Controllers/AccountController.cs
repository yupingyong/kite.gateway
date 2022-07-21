using Kite.Simple.Account.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kite.Simple.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="createAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateAsync(CreateAccountDto createAccount)
        {
            return Ok(createAccount);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="accountName">账号名</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAsync(string accountName)
        {
            return Ok(new 
            {
                AccountName= accountName,
                Password="123456",
                NickName="法外狂徒张三"
            });
        }
    }
}
