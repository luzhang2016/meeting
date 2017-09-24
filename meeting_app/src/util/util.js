import md5 from 'js-md5';
export default {
    install(Vue, options) {
        Vue.prototype.md5 = function() {
            let e = new Date()
            let month = (e.getMonth() + 1) < 10 ? "0" + (e.getMonth() + 1) : (e.getMonth() + 1);
            let day = e.getDate() < 10 ? "0" + e.getDate() : e.getDate();
            let h = e.getHours() < 10 ? "0" + e.getHours() : e.getHours();
            let min = e.getMinutes() < 10 ? "0" + e.getMinutes() : e.getMinutes();
            let s = e.getSeconds() < 10 ? "0" + e.getSeconds() : e.getSeconds();
            let ms = e.getMilliseconds().toString().padStart(3, 0);
            let str = e.getFullYear() + month + day + h + min + s + ms;
            let random = parseInt(100000 * Math.random())
            let sign = str + '-' + random + '-' + '@*api#%^@'
            return md5(sign) + '|' + str + '|' + random;
        }
    }
}