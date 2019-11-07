<template>
  <div>
    <div style="height:37px;"></div>
    <div id="userlist" v-on:scroll="ScrollBlowEvent">
      <ul>
        <li>
          <button type="button" class="btn btn-info" disabled="disabled">{{loginUser}}</button>
        </li>
        <li v-for="user in users" :key="user">
            <button type="button" class="btn btn-outline-info" v-on:click ="$emit('userlistclick',user)">{{user}}</button>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
let p = null;
let timer = false;
export default {
  data() {
    return {
    };
  },
  props: ["users","loginUser"],
  mounted : function(){
    p = this;
    window.setTimeout(function(){
      var scrollelement = document.getElementById("userlist");
      var clientheight = scrollelement.clientHeight;
      var scrollheight = scrollelement.children[0].scrollHeight;
      if(scrollheight<clientheight){
        p.$emit("refreshlist");
      }
    },5000);
  },
  methods:{
    ScrollBlowEvent:function(){
      window.clearTimeout(timer);
      timer = setTimeout(p.event, 1000);
    },
    event:function(){
      var scrollelement = document.getElementById("userlist");
      var clientheight = scrollelement.clientHeight;
      var scrollheight = scrollelement.children[0].scrollHeight;
      var scrolltop = scrollelement.scrollTop;
      if(scrollheight - clientheight - scrolltop <= 20){
        this.$emit('scrolldown');
      }
    }
  }
};
</script>

<style>
#userlist {
  width: 100%;
  height: 100%;
  border-radius: 10px;
  background-color: rgb(243, 243, 243);
  border-style: solid;
  border-color: rgb(23, 162, 184);
  border-width: 2px;
  padding: 5px;
  overflow: auto;
}

#userlist button{
    width: 100%;
}

#userlist button{
  overflow:hidden; 
  text-overflow:ellipsis;
}

#userlist li{
    margin-top: 5px;
}
</style>