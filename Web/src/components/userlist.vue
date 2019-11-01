<template>
  <div>
    <div style="height:37px;"></div>
    <div id="userlist">
      <ul>
        <li v-for="user in users" :key="user">
            <button type="button" class="btn btn-info" v-on:click ="$emit('userlistclick',user)">{{user}}</button>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },
  props: ["users"]
};
</script>

<style>
#userlist {
  width: 100%;
  height: 500px;
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

#userlist li{
    margin-top: 5px;
}
</style>