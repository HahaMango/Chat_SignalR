import Vue from 'vue'
import './css/home.css'
import uistyle from './components/main.vue'

var v = new Vue({
  el: '.back_div',
  data: {
  },
  render(h) {
    return h(uistyle)
  }
})

window.onload = function(){
  if(window.location.hash != ''){
    window.location.href = "/";
  }
}