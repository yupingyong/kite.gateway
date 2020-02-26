using System;

namespace Mango.Module.Message.SignalR
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 绑定用户消息
        /// </summary>
        BindUser=1,
        /// <summary>
        /// 回应通知消息
        /// </summary>
        RespondNotice = 10,
        /// <summary>
        /// 连接回执消息
        /// </summary>
        LineReceipt = 20,
        /// <summary>
        /// 消息发送回执消息
        /// </summary>
        SendReceipt = 21,
        /// <summary>
        /// 绑定用户回执消息
        /// </summary>
        BindUserReceipt = 22
    }
}
