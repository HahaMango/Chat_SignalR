<template>
  <div class="recordblock">
    <div :class="{isright : record.isSend}">
      <div>
        <span class="sender" :class="{isright : record.isSend}">{{record.isSend ? sender : record.communicator}}</span>
      </div>
      <div class="recordcontext" :class="[record.isSend ? 'isright':'isleft']">
        <span>{{record.message}}</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["record","sender"],
  computed:{
  }
};
</script>

<style>
.recordblock{
    height: 65px;
}

.sender {
  display: block;
  font-size : 13px;
  font-weight:bold;
}

.recordcontext {
  padding: 5px;
  border-radius: 5px;
  background-color: rgb(214, 214, 214);
}

.isright {
  float: right;
}

.isleft{
  display: inline-block;
}
</style>