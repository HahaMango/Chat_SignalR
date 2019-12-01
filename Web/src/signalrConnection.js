const sr = require('@aspnet/signalr');
import ChatRecord from "./ChatRecord.js";

let connection = null;

export default {
    //创建连接对象
    Connect: function () {
        connection = new sr.HubConnectionBuilder().withUrl("http://localhost:5000/api/chat").build();
    },
    //注册连接关闭事件
    OnClose : function(callback){
        connection.onclose(callback);
    },
    //注册接收信息事件
    RecevieCallBack: function (callback) {
        connection.on("ReceiveMessage", function(chatRecord){
            var chatr = new ChatRecord(chatRecord.sender,chatRecord.receiver,chatRecord.message,chatRecord.date,chatRecord.isSend);
            callback(chatr);
        });        
    },
    //注册建立通信握手事件
    Handshake : function(callback){
        connection.on("Handshake",function(code){
            callback(code);
        });
    },
    //注册接收在线人数事件
    LoginCount:function(callback){
        connection.on("LoginCount",function(count){
            callback(count);
        })
    },
    //发送信息
    SendMsg : function(chatRecord,successCallback){
        connection.invoke("SendMessage",chatRecord).then(successCallback);
    },
    //建立连接
    StartConnect: function (username) {
        connection.start().then(function () {
            connection.invoke("Handshake",username).catch(function(err){
                console.log(err);
                return;
            });
            console.log("连接到服务器");
        }).catch(function(err){
            console.log(err);
            return;
        });
    }

    
}