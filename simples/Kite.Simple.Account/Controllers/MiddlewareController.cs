using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace Kite.Simple.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiddlewareController : ControllerBase
    {
        /// <summary>
        /// 中间件判断
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostAsync()
        {
            foreach (var item in Request.Headers)
            {
                Console.WriteLine(JsonSerializer.Serialize(item));
            }
            return Ok();
        }
    }
}
