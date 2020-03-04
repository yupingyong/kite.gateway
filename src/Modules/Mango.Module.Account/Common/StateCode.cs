using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Account.Common
{
    public static class StateCode
    {
        private static Dictionary<int, string> _messageType;
        /// <summary>
        /// 系统消息: 0:系统通知
        /// 其它消息: 1.文章点赞消息,10:文档主题点赞消息,11:文档点赞消息
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static string GetMessageType(int messageType)
        {
            if (_messageType==null)
            {
                _messageType = new Dictionary<int, string>();
                _messageType.Add(0, "系统通知");
                _messageType.Add(1, "文章点赞消息");
                _messageType.Add(10, "文档主题点赞消息");
                _messageType.Add(11, "文档点赞消息");
            }
            if (_messageType.ContainsKey(messageType))
            {
                return _messageType[messageType];
            }
            return "无状态";
        }
    }
}
