namespace Kite.Simple.Order.Models
{
    public class CreateOrderDto
    {
        /// <summary>
        /// 下单账号ID
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyCount { get; set; }
    }
}
