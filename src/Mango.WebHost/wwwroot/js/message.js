/// <reference path="../lib/signalr/dist/browser/signalr.min.js" />
//处理Signalr连接
var connection = new signalR.HubConnectionBuilder().withUrl(signalrServerUrl +"/ChatHub").build();
//绑定消息接收方法
connection.on("ReceiveMessage", function (message) {
    console.log(message);
    var msg = JSON.parse(message);
    //平台消息处理
    if (msg.MessageSource == 0) {
        //连接回执消息
        if (msg.MessageType == 98) {
            $("#msg_push_count").text(msg.MessageBody);
        }
    }
});
//开始连接
connection.start().then(function () {
    var message = {
        "MessageId": "",//消息ID
        "MessageType": 1,//消息类型
        "SendUserId": signalRUserId,//消息发送用户ID
        "SendUserNickName": "",//消息发送用户昵称
        "SendUserHeadUrl": "",//消息发送用户头像
        "ReceiveUserId": "0",//消息接收用户ID
        "MessageBody": "ok",//消息内容
        "MessageSource": 1//消息来源
    };
    var msg = JSON.stringify(message);
    //连接成功时发送连接消息
    connection.invoke("ServerTransferMessage", msg).catch(function (err) {
        return console.log(err);
    });
}).catch(function (err) {
    return console.error(err.toString());
});
