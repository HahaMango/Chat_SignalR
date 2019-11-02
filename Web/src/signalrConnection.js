const sr = require('@aspnet/signalr');

let connection = null;

export default {
    Connect: function () {
        connection = new sr.HubConnectionBuilder().withUrl("https://localhost:5001/chat").build();
    },
    OnClose : function(callback){
        connection.onclose(callback);
    },
    RecevieCallBack: function (callback) {
        connection.on("ReceiveMessage", callback);
    },
    SendMsg : function(username,msg,successCallback){
        connection.invoke("SendMessage",username,msg).then(successCallback);
    },
    UserUpdate:function(callback){
        connection.on("UserUpdate",callback);
    },
    StartConnect: function (username ,successCallback) {
        connection.start().then(function () {
            connection.invoke("AssociatedConnectId",username).catch(function(err){
                return;
            });
            successCallback();
            console.log("已连接");
        }).catch(function(err){
            console.log(err);
            return;
        });
    }
}