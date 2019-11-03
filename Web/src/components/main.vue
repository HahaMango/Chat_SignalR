<template>
  <div>
    <div id="login" v-if="currentRoute === ''">
      <loginview v-on:loginclick="LoginEvent" />
    </div>
    <div id="chat_div" v-if="currentRoute === '#chat'">
      <div class="maincont">
        <div class="allsend">
          <chattag :userMap="follow" class="chatview1" :loginuser="loginUser" v-on:sendrecord="SendAllEvent"/>
        </div>
        <div class="sigsend">
          <chattag
            :userMap="userMap"
            class="chatview2"
            :loginuser="loginUser"
            v-on:sendrecord="SendMessageEvent"
            v-on:closetag="CloseTagEvent"
          />
        </div>
        <div class="usersview">
          <userlist 
            :users="userlist" 
            :loginUser="loginUser" 
            class="chatusers" 
            v-on:userlistclick="SelectUserEvent" 
            v-on:scrolldown="ScrollUpdate"
            v-on:refreshlist="RefreshUpdate"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import chattag from "./chatTag.vue";
import loginview from "./login.vue";
import userlist from './userlist.vue'
import ChatRecord from "../ChatRecord.js";
import signalr from '../signalrConnection.js';

let p = null;

export default {
  data() {
    return {
      currentRoute: window.location.hash,
      loginUser: null,
      userMap: {
      },
      follow: {
        "广播聊天室": [
        ]
      },
      userlist:[]
    };
  },
  computed:{
  },
  mounted: function() {
    p = this;
    this.hashevent();
    this.appclose();
  },
  methods: {
    hashevent: function() {
      window.onhashchange = function() {
        if (p.loginUser == null) {
          window.location.href = "/";
          return;
        }
        p.currentRoute = window.location.hash;
      };
    },
    appclose: function(){
      window.onbeforeunload = function(){
        var user = p.loginUser;
        if(user != null && window.location.hash == "#chat"){
          p.LogoutEvent(user);
        }
      };
    },
    LoginEvent: function(value) {
      if (value == null && value == "ALL") {
        return;
      }
      
      var xmlhttp = new XMLHttpRequest();
      xmlhttp.open("POST","https://localhost:5001/login",true);
      xmlhttp.setRequestHeader("Content-type","application/x-www-form-urlencoded");
      xmlhttp.send("username="+ value);
      xmlhttp.onreadystatechange = function(){
        if(xmlhttp.readyState == 4 && xmlhttp.status == 200){
            signalr.Connect();
            signalr.RecevieCallBack(p.RecevieEvent);
            signalr.OnClose(function(){
              p.LogoutEvent(p.loginUser);
              window.location.href = "/";
            });
            signalr.StartConnect(value,function(){
              p.loginUser = value;
              window.location.href = "#chat";
            });
        }
      };    
    },
    LogoutEvent:function(value){
      var xmlhttp = new XMLHttpRequest();
      xmlhttp.open("POST","https://localhost:5001/logout",false);
      xmlhttp.setRequestHeader("Content-type","application/x-www-form-urlencoded");
      xmlhttp.send("username="+value);
      this.loginUser == null;
    },
    SendAllEvent:function(value){
      signalr.SendMsg("ALL",value.message,function(){
        value.date = new Date();
        p.follow["广播聊天室"].push(value);
      });
    },
    SendMessageEvent: function(value) {
      signalr.SendMsg(value.communicator,value.message,function(){
        value.date = new Date();
        p.userMap[value.communicator].push(value);
      });
    },
    RecevieEvent:function(username,msg,isAll){
      if(username == null || msg == null){
        return;
      }

      if(isAll){
        this.follow["广播聊天室"].push(new ChatRecord(username,msg,new Date(),false));
      }else{
        var msglist = this.userMap[username];
        if(msglist != null){
          var chatRecord = new ChatRecord(username,msg,new Date(),false);
          msglist.push(chatRecord);
        }
      }
    },
    ScrollUpdate:function(page){
      this.GetUserList(page,15,function(userlist){
        for(var index in userlist){
          var username = userlist[index];
          if(username != p.loginUser){
            p.userlist.push(username);
          }
        }
      });
    },
    RefreshUpdate:function(){
      this.GetUserList(1,15,function(userlist){
        for(var index in userlist){
          var username = userlist[index];
          if(username != p.loginUser){
            p.userlist.push(username);
          }
        }
      });
    },
    GetUserList:function(page,count,resultCallBack){
      var xmlhttp1 = new XMLHttpRequest();
      xmlhttp1.open("GET","https://localhost:5001/userlist/"+page+"/"+count,true);
      xmlhttp1.send();
      xmlhttp1.onreadystatechange = function(){
        if(xmlhttp1.readyState == 4 && xmlhttp1.status == 200){
           var userlist = eval(xmlhttp1.responseText);
           resultCallBack(userlist);
        }
      };
    },
    SelectUserEvent:function(user){
      if(user==null || this.userMap[user]!=null){
        return;
      }
      this.$set(this.userMap,user,[]);
    },
    CloseTagEvent:function(user){
      if(user==null){
        return;
      }
      this.$delete(this.userMap,user); 
    }
  },
  components: {
    chattag,
    loginview,
    userlist
  }
};


</script>>

<style>
#chat_div {
  padding: 5px 0px 0px 0px;
  background-color: rgb(44, 44, 44);
}

.chatview1 {
  width: 500px;
}

.chatview2 {
  width: 500px;
  margin-left: 40px;
}

.chatusers{
  width: 150px;
  margin-left: 40px;
}

.allsend {
  float: left;
}

.sigsend {
  float: left;
}

.usersview{
  float: left;
}

.maincont {
  margin: 0px auto 0px auto;
  width: 1300px;
  padding: 20px;
}
</style>