import axios from 'axios'
import qs from 'qs'
import md5 from 'js-md5';

const apiUrl = '';
// axios.defaults.headers.post['Content-Type'] = 'application/json';
function token() {
    let e = new Date()
    let month = (e.getMonth() + 1) < 10 ? "0" + (e.getMonth() + 1) : (e.getMonth() + 1);
    let day = e.getDate() < 10 ? "0" + e.getDate() : e.getDate();
    let h = e.getHours() < 10 ? "0" + e.getHours() : e.getHours();
    let min = e.getMinutes() < 10 ? "0" + e.getMinutes() : e.getMinutes();
    let s = e.getSeconds() < 10 ? "0" + e.getSeconds() : e.getSeconds();
    let ms = e.getMilliseconds();
    ms = ms >= 100 ? ms : (ms < 10 ? "00" + ms : "0" + ms);
    let str = e.getFullYear() + month + day + h + min + s + ms;
    let random = parseInt(100000 * Math.random())
    let sign = str + '-' + random + '-' + '@*api#%^@'
    return md5(sign) + '|' + str + '|' + random;
}

function get(url) {
    return axios.get(apiUrl + url + '&token=' + token()).then(res => {
        if (res.status !== 200) return null;
        if (!res.data.success) return alert(res.data.message);
        return res.data.data;
    }).catch(err => {
        console.error(err);
        return null;
    });
};

function put(url) {
    // console.log('request url:', url);
    return axios.put(apiUrl + url + '&token=' + token()).then(res => {
        if (res.status !== 200) return null;
        if (!res.data.success) return null;
        return res.data.data;
    }).catch(err => {
        console.error(err); //输出日志
        return null;
    })
};

class apiService {
    constructor(Vue) {
        //  this.Vue = new Vue();
    }
    getListSimple(sTime, eTime) {
        return get('/api/meeting/listSimple?sTime=' + sTime + '&eTime=' + eTime)
    }

    getMeeting(uuid) {
        return get('/api/meeting?UUID=' + uuid)
    }

    getFiles(uuid, flag) {
        return get('/api/meeting/files?uuid=' + uuid + '&flag=' + flag)
    }
    downLoad(fileUUID, username) {
        return put('/api/meeting/download?fileUUID=' + fileUUID + '&downloadUser=' + username)
    }
    getSignIn(uuid, phone) {
        return get('/api/meeting/signin?uuid=' + uuid + '&mobile=' + phone)
    }
    SignIn(uuid, phone, addr) {
        return put('/api/meeting/signin?uuid=' + uuid + '&mobile=' + phone + '&signInAddr=' + addr)
    }
    getSignInList(uuid) {
        return get('/api/meeting/signInList?uuid=' + uuid)
    }
}

function plugin_api(Vue) {

    let api = new apiService(Vue);
    Vue.plugin_api = api;

    Object.defineProperties(Vue.prototype, {
        $plugin_api: {
            get: function() {
                return api;
            }
        }
    });
}


export default plugin_api;