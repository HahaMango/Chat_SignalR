<template>
  <div>
    <div id="head">
      <div id="title_div">
        <b>
          <span>FlashChat</span>
        </b>
        <span>{{loginUser}}</span>
      </div>
    </div>
    <div id="login" v-if="currentRoute === ''">
      <loginview v-on:loginclick="LoginEvent" />
    </div>
    <div id="chat_div" v-if="currentRoute === '#chat'">
      <div id="maincont" v-bind:style="{height: maincontHeight + 'px'}">
        <div class="allsend">
          <chattag
            :userMap="follow"
            class="chatview1"
            :loginuser="loginUser"
            v-on:sendrecord="SendAllEvent"
          />
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
            class="chatusers"
            v-on:userlistclick="SelectUserEvent"
            v-on:scrolldown="ScrollUpdate"
            v-on:refreshlist="RefreshUpdate"
          />
        </div>
      </div>
    </div>
    <div id="footer">
      <div class="center">
        <a href="https://github.com/HahaMango/Chat_SignalR" target="_blank">
          <img src="githubico.png" />
        </a>
        <span>
          <i>power by .NET Core / Vue.js</i>
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import chattag from "./chatTag.vue";
import loginview from "./login.vue";
import userlist from "./userlist.vue";
import ChatRecord from "../ChatRecord.js";
import signalr from "../signalrConnection.js";

let p = null;

export default {
  data() {
    return {
      currentRoute: window.location.hash,
      loginUser: null,
      userMap: {},
      follow: {
        广播聊天室: []
      },
      userlist: []
    };
  },
  computed: {
    maincontHeight: function() {
      var clientHeight = document.documentElement.clientHeight;
      return clientHeight - 270;
    },
    titleWidth: function() {
      var clientWidth = document.documentElement.clientWidth;
      return clientWidth - 400;
    },
    chatviewWidth: function() {
      var width = (this.titleWidth - 230) / 2;
      return width;
    }
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

        signalr.Connect();
        signalr.RecevieCallBack(p.RecevieEvent);
        signalr.Handshake(function(code) {
          if (code == 1) {
            console.log("已经连接");
          }else if(code == -1){
            console.log("出现错误");
            
          }
        });
        signalr.OnClose(function() {
          p.LogoutEvent(p.loginUser);
          window.location.href = "/";
        });
        signalr.StartConnect(p.loginUser);
      };
    },
    appclose: function() {
      window.onbeforeunload = function() {
        var user = p.loginUser;
        if (user != null && window.location.hash == "#chat") {
          p.LogoutEvent(user);
        }
      };
    },
    LoginEvent: function(value) {
      if (value == null && value == "广播聊天室" && value == "SYSTEM") {
        return;
      }

      var xmlhttp = new XMLHttpRequest();
      xmlhttp.open("POST", "http://localhost:5000/api/login", true);
      xmlhttp.setRequestHeader(
        "Content-type",
        "application/x-www-form-urlencoded"
      );
      xmlhttp.send("username=" + value);
      xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
          p.loginUser = value;
          window.location.href = "#chat";
        }else{
          alert("登陆失败");
        }
      };
    },

    LogoutEvent: function(value) {
      var xmlhttp = new XMLHttpRequest();
      xmlhttp.open("POST", "http://localhost:5000/api/logout", false);
      xmlhttp.setRequestHeader(
        "Content-type",
        "application/x-www-form-urlencoded"
      );
      xmlhttp.send("username=" + value);
      this.loginUser == null;
    },
    SendAllEvent: function(value) {
      signalr.SendMsg(value, function() {
        value.Date = new Date();
        p.follow["广播聊天室"].push(value);
      });
    },
    SendMessageEvent: function(value) {
      signalr.SendMsg(value, function() {
        value.Date = new Date();
        p.userMap[value.Receiver].push(value);
      });
    },
    RecevieEvent: function(chatRecord) {
      if (chatRecord == null) {
        return;
      }
      if (chatRecord.Sender == "SYSTEM") {
        window.alert(chatRecord.message);
        return;
      }
      if (chatRecord.Receiver == "广播聊天室") {
        this.follow["广播聊天室"].push(chatRecord);
      } else {
        var msglist = this.userMap[chatRecord.Sender];
        if (msglist != null) {
          msglist.push(chatRecord);
        } else if (msglist == null) {
          this.$set(this.userMap, chatRecord.Sender, [chatRecord]);
        }
      }
    },
    ScrollUpdate: function() {
      var userlistcount = this.userlist.length;
      this.GetUserList(userlistcount, 10, function(userlist) {
        for (var index in userlist) {
          var username = userlist[index];
          p.userlist.push(username);
        }
      });
    },
    RefreshUpdate: function() {
      var userlistcount = this.userlist.length;
      this.GetUserList(userlistcount, 10, function(userlist) {
        for (var index in userlist) {
          var username = userlist[index];
          p.userlist.push(username);
        }
      });
    },
    GetUserList: function(page, count, resultCallBack) {
      var xmlhttp1 = new XMLHttpRequest();
      xmlhttp1.open(
        "GET",
        "http://localhost:5000/api/userlist/" + page + "/" + count,
        true
      );
      xmlhttp1.send();
      xmlhttp1.onreadystatechange = function() {
        if (xmlhttp1.readyState == 4 && xmlhttp1.status == 200) {
          var userlist = eval(xmlhttp1.responseText);
          resultCallBack(userlist);
        }
      };
    },
    SelectUserEvent: function(user) {
      if (
        user == null ||
        this.userMap[user] != null ||
        this.loginUser == user
      ) {
        return;
      }
      this.$set(this.userMap, user, []);
    },
    CloseTagEvent: function(user) {
      if (user == null) {
        return;
      }
      this.$delete(this.userMap, user);
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
  /*width: 400px;*/
  height: 100%;
}

.chatview2 {
  /*width: 400px;*/
  /*margin-left: 40px;*/
  height: 100%;
}

.chatusers {
  /*width: 150px;*/
  /*margin-left: 40px;*/
  height: 100%;
}

.allsend {
  float: left;
  height: 100%;
  width: 40%;
}

.sigsend {
  float: left;
  height: 100%;
  width: 40%;
  margin-left: 3%;
}

.usersview {
  float: left;
  height: 100%;
  width: 14%;
  margin-left: 3%;
}

#maincont {
  margin: 0px 100px 0px 100px;
  /*width: 1100px*/
  padding: 5px 10px 20px 10px;
  margin-bottom: 90px;
}

#footer .center {
  width: 250px;
  margin: 0px auto 0px auto;
}

#footer .center span {
  display: inline-block;
  margin: 10px;
  color: gray;
}

#title_div {
  padding: 10px 10px 10px 10px;
  height: 56.8px;
  color: rgb(23, 162, 184);
  margin: 20px 100px 20px 100px;
  background-color: rgba(23, 162, 184, 0.33);
  border-radius: 5px;
  box-shadow: 0px 4px 24px #121212;
}

#title_div > b {
  margin-left: 30px;
  font-size: 25px;
  float: left;
}

#title_div > span {
  padding-top: 12px;
  float: right;
}
</style>