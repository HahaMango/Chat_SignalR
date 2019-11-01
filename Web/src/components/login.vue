<template>
  <div>
    <div class="head_div"></div>
    <div class="login_div">
      <form>
        <div class="form-group row">
          <label for="username" class="col-sm-4 col-form-label namelabel">用户名</label>
          <input type="text" class="form-control col-sm-8" id="username" placeholder="输入临时用户名" v-model="username"/>
        </div>
        <button
          id="loginbutton"
          type="button"
          class="btn btn-info login_button"
          v-on:click="$emit('loginclick',username)"
        >登陆</button>
      </form>
    </div>
  </div>
</template>

<script>
export default {
    methods:{
  },
  data (){
    return {
      username : null
    }
  }
}
</script>

<style>
.head_div{
    height: 200px;
}

.namelabel{
  color: rgb(23, 162, 184);
}

.login_div{
    border-style: solid none solid none;
    border-top-color: rgb(168, 168, 168);
    border-bottom-color: rgb(168, 168, 168);
    margin: auto;
    width: 400px;
    height: 160px;
    padding: 35px 50px 0px 50px;
}

.login_button{
    margin-left : auto;
    margin-right: auto;
    display: block;
}
</style>