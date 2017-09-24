const headComponent = require('./header.vue')
const myhead = {
    install: function(Vue) {
        Vue.component('myhead', headComponent)
    }
}
module.exports = myhead