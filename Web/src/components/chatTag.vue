<template>
  <div>
    <div class="btn-group tabroot" role="group" aria-label="Basic example">
      <button
        type="button"
        v-for="(user, userkey) in userMap"
        :class="['btn btn-outline-info',{active:currentTab === userkey}]"
        :key="userkey"
        v-on:click="currentTab = userkey"
      >
        {{userkey}}
        <button type="button" class="close" aria-label="关闭" v-on:click="$emit('closetag',userkey)">
          <span aria-hidden="true" style="color:white;">&times;</span>
        </button>
      </button>
    </div>
    <chatview class="chat_view" :chatrecords="getrecord" :sendname="loginuser" v-on:sendmsg="getRecordObject"/>
  </div>
</template>

<script>
import chatview from './chatview.vue';
import ChatRecord from '../ChatRecord.js';

export default {
  props: ["userMap","loginuser"],
  data () {
      return {
        currentTab:'广播聊天室',
      }
  },
  methods:{
      getRecordObject:function (value) {
          var record = new ChatRecord(this.currentTab,value,null,true);
          this.$emit('sendrecord',record);
      }
  },
  components:{
      chatview
  },
  computed:{
      getrecord:function(){
          return this.userMap[this.currentTab];
      }
  }
};
</script>

<style>
.userTab {
  border-style: solid;
  border-color: gray;
  padding: 0px 5px 0px 5px;
}

.close{
    margin-left: 10px;
}

.tabroot{
    margin-left: 15px;
    height: 37px;
}

.tabroot .btn {
    border-bottom-style: none;
}

.tabroot .btn:first-child{
    border-bottom-left-radius: 0;
}

.tabroot .btn:last-child{
    border-bottom-right-radius: 0;
}

.chat_view{
  height: 100%;
}
</style>