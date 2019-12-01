<template>
  <div class="recordblock">
    <div :class="{isright : record.IsSend}">
      <div>
        <span class="sender" :class="{isright : record.IsSend}">{{record.Sender}}</span>
      </div>
      <div class="recordcontext" :class="[record.IsSend ? 'isright':'isleft']">
        <span>{{record.Message}}</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["record","loginuser"],
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