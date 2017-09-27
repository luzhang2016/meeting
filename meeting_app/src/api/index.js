import axios from 'axios'
import qs from 'qs'

const apiUrl = 'http://localhost:80';
// axios.defaults.headers.post['Content-Type'] = 'application/json';

function get(url) {
    console.log('request url:', apiUrl + url)
    return axios.get(apiUrl + url).then(res => {
        // if (res.status !== 200) return null;
        // if (!res.data.success) return alert(res.data.message);
        return res.data;
    }).catch(err => {
        console.error(err);
        return null;
    });
};

function put(url) {
    // console.log('request url:', url);
    return axios.put(apiUrl + url).then(res => {
        // if (res.status !== 200) return null;
        // if (!res.data.success) return null;
        return res.data;
    }).catch(err => {
        console.error(err); //输出日志
        return null;
    })
};

class apiService {
    constructor(Vue) {
        //  this.Vue = new Vue();
    }
    getListSimple() {
        return get("/meeting/php/index/list.php")
    }

    getMeeting(mid, phone = '') {
        return get('/meeting/php/index/meetdetail.php?mid=' + mid + '&phone=' + phone)
    }
    getAgenda(mid) {
        return get('/meeting/php/index/meetagenda.php?mid=' + mid)
    }
    getMember(mid) {
        return get('/meeting/php/index/member.php?mid=' + mid)
    }

    getFiles(mid, flag) {
        return get('/meeting/php/index/download.php?mid=' + mid + '&flag=' + flag)
    }
    downLoad(fileUUID, username) {
        return put('/api/meeting/download?fileUUID=' + fileUUID + '&downloadUser=' + username)
    }
    getSignIn(mid, phone) { //签到成功后
        return get('/api/meeting/signin?uuid=' + mid + '&mobile=' + phone)
    }
    SignIn(mid, phone, addr) { //签到
        return put('/meeting/php/index/signin.php?mid=' + mid + '&mobile=' + phone + '&signInAddr=' + addr)
    }
    getSignInList(mid) { //签到详情
        return get('/api/meeting/signInList?uuid=' + mid)
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