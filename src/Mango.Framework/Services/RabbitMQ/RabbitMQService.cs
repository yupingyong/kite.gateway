using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Mango.Framework.Services.RabbitMQ
{
    public class RabbitMQService:IRabbitMQService
    {
        private IModel _channel;
        public RabbitMQService(RabbitMQOptions options)
        {
            try
            {
                //创建连接工厂
                ConnectionFactory connectionFactory = new ConnectionFactory
                {
                    HostName = options.HostName,
                    UserName = options.UserName,//用户名    
                    Password = options.Password,//密码 
                    Port = options.Port
                };
                //创建连接
                var connection = connectionFactory.CreateConnection();
                //创建通道
                _channel = connection.CreateModel();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="queueName">对列名</param>
        /// <param name="durable">是否持久化</param>
        /// <param name="exclusive">是否独有</param>
        /// <param name="autoDelete">是否自动删除</param>
        public void CreateQueue(string queueName,bool durable,bool exclusive,bool autoDelete)
        {
            if (_channel == null)
                return;
            _channel.QueueDeclare(queueName, durable: durable, exclusive: exclusive ,autoDelete: autoDelete, null);
        }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        public void BasicPublish(string routingKey,byte[] body)
        {
            if (_channel == null)
                return;
            _channel.BasicPublish("", routingKey: routingKey, null, body: body);
        }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        public void BasicPublish(string exchange, string routingKey, byte[] body)
        {
            if (_channel == null)
                return;
            _channel.BasicPublish(exchange: exchange, routingKey: routingKey, null, body: body);
        }
        /// <summary>
        /// 手动确认消息已经消费
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="multiple"></param>
        public void BasicAck(ulong deliveryTag,bool multiple)
        {
            if (_channel == null)
                return;
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: multiple);
        }
        /// <summary>
        /// 创建消息消费事件
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="autoAck"></param>
        /// <param name="eventHandler"></param>
        public void CreateConsumeEvent(string queueName,bool autoAck, EventHandler<BasicDeliverEventArgs> eventHandler)
        {
            if (_channel == null)
                return;
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += eventHandler;
            //消费者开启监听
            _channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
        }
    }
}
