using Kite.Simple.Order.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kite.Simple.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateAsync(CreateOrderDto createOrder)
        {
            var result = new
            {
                AccountId = createOrder.AccountId,
                ProductId = createOrder.ProductId,
                Price = createOrder.Price,
                BuyCount = createOrder.BuyCount,
                OrderNo = "T202207090001",
                TotalAmount= createOrder.Price* createOrder.BuyCount
            };
            return Ok(result);
        }
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAsync(string orderNo)
        {
            var result = new
            {
                AccountId = 1,
                ProductId = 1,
                Price = 9.9,
                BuyCount = 10,
                OrderNo = orderNo,
                TotalAmount = 99
            };
            return Ok(result);
        }
    }
}
