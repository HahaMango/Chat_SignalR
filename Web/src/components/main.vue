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
          <userlist :users="userlist" class="chatusers" v-on:userlistclick="SelectUserEvent"/>
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
    this.hashevent();
    p = this;
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
    LoginEvent: function(value) {
      if (value == null) {
        return;
      }
      /*
      var xmlhttp = new XMLHttpRequest();
      xmlhttp.open("POST","login",true);
      xmlhttp.send(value);
      xmlhttp.onreadystatechange = function(){
        if(xmlhttp.readyState == 4 && xmlhttp.status == 200){
          if(xmlhttp.responseText == "OK"){
            signalr.Connect();
            signalr.RecevieCallBack(this.RecevieEvent);
            signalr.UserUpdate(this.UserUpdate);
            if(signalr.StartConnect(value)){
              this.loginUser = value;
              window.location.href = "#chat";
            }
          }
        }
      };
      */
      this.loginUser = value;
      window.location.href = "#chat";
    },
    SendAllEvent:function(value){
      value.date = new Date();
      this.follow["广播聊天室"].push(value);
    },
    SendMessageEvent: function(value) {
      if(signalr.SendMsg(value.communicator,value.message)){
        value.date = new Date();
        this.userMap[value.communicator].push(value);
      }
    },
    RecevieEvent:function(username,msg){
      if(username == null || msg == null){
        return;
      }
      var msglist = this.userMap[username];
      if(msglist != null){
        var chatRecord = new ChatRecord(username,msg,new Date(),false);
        msglist.push(chatRecord);
      }
    },
    UserUpdate:function(users){
      for(var user in users){
        this.userlist.push(user);
      }
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