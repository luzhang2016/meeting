// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import Mint from 'mint-ui';
import 'mint-ui/lib/style.css';
import axios from 'axios'
import filters from './filters'
import 'font-awesome/css/font-awesome.min.css'
import Chart from 'Chart.js'
// import myFunc from './util/util'
import Loading from './components/loading'
import myhead from './components/header'
import api from './api/index'


Object.keys(filters).forEach((key) => Vue.filter(key, filters[key]));

Vue.use(Mint);
// Vue.use(myFunc);
Vue.use(Loading);
Vue.use(myhead);
Vue.use(api);

Vue.config.productionTip = false

//axios的一些配置，比如发送请求显示loading，请求回来loading消失之类的
// axios.interceptors.request.use(function(config) { //配置发送请求的信息
//     store.dispatch('showLoading')
//     return config;
// }, function(error) {
//     return Promise.reject(error);
// });

// axios.interceptors.response.use(function(response) { //配置请求回来的信息
//     store.dispatch('hideLoading')
//     return response;
// }, function(error) {

//     return Promise.reject(error);
// });

Vue.prototype.$http = axios

/* eslint-disable no-new */
new Vue({
    el: '#app',
    router,
    template: '<App/>',
    components: { App }
})