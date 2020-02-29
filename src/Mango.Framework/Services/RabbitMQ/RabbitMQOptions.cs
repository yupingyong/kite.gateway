using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Services.RabbitMQ
{
    public class RabbitMQOptions
    {
        /// <summary>
        /// 服务器地址(IP)
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 端口号(默认:5672)
        /// </summary>
        public int Port { get; set; }
    }
}
