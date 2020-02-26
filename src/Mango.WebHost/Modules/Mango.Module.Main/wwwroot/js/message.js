/// <reference path="../lib/signalr/signalr.min.js" />
//处理Signalr连接
var connection = new signalR.HubConnectionBuilder().withUrl("/MessageHub").build();
//绑定消息接收方法
connection.on("ReceiveMessage", function (message) {
    console.log(message);
    var res = JSON.parse(message);
   
    switch (res.MessageType) {
        case 1:
            break;
        case 10:
            $("#msg_push_count").text(res.MessageBody);
            break;
        case 20:
            var msg = {
                MessageType: 1,
                MessageBody: 'ok',
                SendUserId: signalRUserId,
                ReceveUserId:'0'
            };
            var sendMsg = JSON.stringify(msg);
            connection.invoke("ServerTransferMessage", sendMsg).catch(function (err) {
                return console.log(err);
            });
            break;
        case 21:

            break;
        case 22:

            break;
    }
    console.log(message);
    
});
//开始连接
connection.start().catch(function (err) {
    return console.error(err.toString());
});