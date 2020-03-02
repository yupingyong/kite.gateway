using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;
using NLog;
using Microsoft.AspNetCore.SignalR;
namespace Mango.Module.Message.SignalR
{
    /// <summary>
    /// 消息处理
    /// </summary>
    public class MessageDealWidth
    {
        private static object[] _objData = new object[1];
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public static async Task DealWidth(string message, MessageHub hub)
        {
            await Task.Run(() => {
                try
                {
                    if (!message.Contains("{") && !message.Contains("}"))
                    {
                        return;
                    }
                    MessageData data = JsonConvert.DeserializeObject<MessageData>(message);
                    if (data != null)
                    {
                        ConnectionUser connectionUser = ConnectionManager.ConnectionUsers.Where(q => q.UserId == data.SendUserId).FirstOrDefault();
                        MessageData sendMsg = null;

                        switch (data.MessageType)
                        {
                            case MessageType.BindUser:
                                //处理连接消息
                                if (connectionUser == null)
                                {
                                    connectionUser = ConnectionManager.ConnectionUsers.Where(q => q.UserId == string.Empty).FirstOrDefault();
                                    connectionUser.UserId = data.SendUserId;
                                }
                                connectionUser.ConnectionIds.Add(hub.Context.ConnectionId);
                                //处理发送回执消息
                                sendMsg = new MessageData();
                                sendMsg.MessageBody = "success";
                                sendMsg.MessageType = MessageType.BindUserReceipt;
                                sendMsg.SendUserId = "0";
                                sendMsg.ReceveUserId = data.ReceveUserId;
                                _objData[0] = JsonConvert.SerializeObject(sendMsg);

                                hub.Clients.Client(hub.Context.ConnectionId).SendCoreAsync("ReceiveMessage", _objData, CancellationToken.None);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Info($"error-receive-data:{message}");
                    _logger.Error(ex, "MessageDealWidth-DealWidth:Error");
                }
            });
        }
    }
}
