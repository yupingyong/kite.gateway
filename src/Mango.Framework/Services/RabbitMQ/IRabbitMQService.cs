using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client.Events;

namespace Mango.Framework.Services.RabbitMQ
{
    public interface IRabbitMQService
    {
        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="queueName">对列名</param>
        /// <param name="durable">是否持久化</param>
        /// <param name="exclusive">是否独有</param>
        /// <param name="autoDelete">是否自动删除</param>
        void CreateQueue(string queueName, bool durable, bool exclusive, bool autoDelete);
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        void BasicPublish(string routingKey, byte[] body);
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        void BasicPublish(string exchange, string routingKey, byte[] body);
        /// <summary>
        /// 创建消息消费事件
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="autoAck"></param>
        /// <param name="eventHandler"></param>
        void CreateConsumeEvent(string queueName, bool autoAck, EventHandler<BasicDeliverEventArgs> eventHandler);
    }
}
