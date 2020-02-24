using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Message.SignalR
{
    public class ConnectionUser
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// SignalR连接ID
        /// </summary>
        public List<string> ConnectionIds { get; set; }
    }
}
