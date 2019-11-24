const sr = require('@aspnet/signalr');
import ChatRecord from "./ChatRecord.js";

let connection = null;

export default {
    Connect: function () {
        connection = new sr.HubConnectionBuilder().withUrl("/api/chat").build();
    },
    OnClose : function(callback){
        connection.onclose(callback);
    },
    RecevieCallBack: function (callback) {
        connection.on("ReceiveMessage", function(sender,receiver, msg){
            var record = new ChatRecord(sender,receiver,msg,new Date(),false);
            callback(record);
        });
    },
    SendMsg : function(chatRecord,successCallback){
        connection.invoke("SendMessage",chatRecord.sender,chatRecord.receiver,chatRecord.message).then(successCallback);
    },
    UserUpdate:function(callback){
        connection.on("UserUpdate",callback);
    },
    StartConnect: function (username ,successCallback) {
        connection.start().then(function () {
            connection.invoke("AssociatedConnectId",username).catch(function(err){
                console.log(err);
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