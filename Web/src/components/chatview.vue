<template>
  <div>
    <div id="chatlist">
      <ul>
        <li v-for="record in chatrecords" :key="record.Id">
          <chatitem :record="record" :sender="loginuser" />
        </li>
      </ul>
    </div>
    <div id="messageinput">
      <div>
        <input type="text" class="form-control" placeholder="请输入..." v-model="message" v-on:keyup.enter="getRecordObject"/>
        <button type="button" class="btn btn-success" v-on:click="getRecordObject">发送</button>
      </div>
    </div>
  </div>
</template>

<script>
import chatitem from "./chatItem.vue";

export default {
data (){
    return {
      recordKey : 1,
      message:null
    }
  },
  props: ["chatrecords", "loginuser"],
  components: {
    chatitem
  },
  methods:{
      getRecordObject:function () {
          if(this.message == null || this.chatrecords == null){
              return;
          }
          this.$emit('sendmsg',this.message);
          this.message = '';
      },
      Inc:function(value){
        return ++this.recordKey;
      }
  }
};
</script>

<style>
.form-control {
  width: 75%;
}

#messageinput {
  margin-top: 10px;
  width: 100%;
  border-radius: 10px;
  background-color: rgb(243, 243, 243);
  border-style: solid;
  border-color: rgb(23, 162, 184);
  border-width: 2px;
  padding: 5px;
  height: 50px;
}

#messageinput input {
  display: inline-block;
}

#messageinput button{
    margin-left: 10px;
    margin-top: -5px;
    width:20%;
}

#chatlist {
  width: 100%;
  height: 100%;
  border-radius: 10px;
  background-color: rgb(243, 243, 243);
  border-style: solid;
  border-color: rgb(23, 162, 184);
  border-width: 2px;
  padding: 20px;
  overflow: auto;
}

ul {
  padding: 0px;
}

ul li {
  list-style-type: none;
}
</style>