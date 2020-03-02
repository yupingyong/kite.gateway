using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
namespace Mango.Module.Message.SignalR
{
    public class MessageHub:Hub
    {
        /// <summary>
        /// 服务器端中转消息处理方法
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task ServerTransferMessage(string message)
        {
            await MessageDealWidth.DealWidth(message, this);
        }
        /// <summary>
        /// 用户连接方法重写
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            object[] _objData = new object[1];
            var sendMsg = new MessageData();
            sendMsg.MessageBody = "success";
            sendMsg.MessageType = MessageType.LineReceipt;
            sendMsg.SendUserId = "0";
            sendMsg.ReceveUserId = "0";
            _objData[0] = JsonConvert.SerializeObject(sendMsg);

            this.Clients.Client(this.Context.ConnectionId).SendCoreAsync("ReceiveMessage", _objData, CancellationToken.None);
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 用户断开连接方法重写
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var connectionUsers=  ConnectionManager.ConnectionUsers.Where(q => q.ConnectionIds.Where(c => c == this.Context.ConnectionId).FirstOrDefault() != null).FirstOrDefault();
                if (connectionUsers != null)
                {
                    connectionUsers.ConnectionIds.Remove(this.Context.ConnectionId);
                    if (connectionUsers.ConnectionIds.Count <= 0)
                    {
                        connectionUsers.UserId = string.Empty;
                        connectionUsers.ConnectionIds = new List<string>();
                    }
                }
            }
            catch
            {

            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
