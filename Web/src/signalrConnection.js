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
    SendMsg : function(username,msg){
        connection.invoke("SendMessage",username,msg).catch(function(err){
            console.log(err);
            return false;
        });
        return true;
    },
    UserUpdate:function(callback){
        connection.on("UserUpdate",callback);
    },
    StartConnect: function (username) {
        connection.start().then(function () {
            console.log("已连接");
        }).catch(function(err){
            console.log(err);
            return false;
        });

        connection.invoke("Identify",username).catch(function(err){
            console.log(err);
            return false;
        });
        return true;
    }
}