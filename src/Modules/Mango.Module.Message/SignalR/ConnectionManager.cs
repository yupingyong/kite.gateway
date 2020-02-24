using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Message.SignalR
{
    public class ConnectionManager
    {
        /// <summary>
        /// 链接用户管理
        /// </summary>
        public static List<ConnectionUser> ConnectionUsers { get; set; } = new List<ConnectionUser>();
    }
}
