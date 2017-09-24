import axios from 'axios';

let base = '';

function get(url) {
    console.log('request url:', url);
    return axios.get(base + url)
        .then(res => {
            if (res.status !== 200) return null;
            if (!res.data.success) return alert(res.data.message);
            return res.data.data;
        }).catch(err => {
            console.error(err);
            return null;
        });
};

function post(url, data) {
    console.log('request url:', url);
    return axios.post(base + url, data)
        .then(res => {
            if (res.status !== 200) return null;
            if (!res.data.success) return null;
            return res.data.data;

        }).catch(err => {
            console.error(err); //输出日志
            return null;
        })
};

function put(url,data){
      console.log('request url:',url);
    return axios.put(base + url,data)
    .then(res =>{
        if(res.sratus !==200) return null;
        if(!res.data.success) return null;
        return res.data.data;
    }).catch(err =>{
        console.error(err);
        return null;
    })
}

export const requestLogin = (params) => {
    return get('/api/meeting/login?token=' + params.token + '&username=' + params.username + '&password=' + params.password);
};
export const MeetingList = (params) => {
    return get('/api/meeting/list?name=' + params.name + '&token=' + params.token + '&meetingStatus=' + params.meetingStatus + '&publishStatus=' + params.publishStatus);
};
export const removeMeeting = params => {
    return axios.delete(`${base}/api/meeting/?uuid=` + params.uuid + '&token=' + params.token);
};
export const getMeetingList = (params) => {
    return get('/api/api/meeting/?uuid=' + params.uuid + '&token=' + params.token);
};
export const addMeeting = (params) => {
    return post('/api/meeting', params);
};
export const MeetingListPage = (params) => {
    return get('/api/meeting/listpage?name=' + params.name + '&token=' + params.token + '&publishStatus=' + params.publishStatus + '&meetingStatus=' + params.meetingStatus + '&page=' + params.page + '&pagesize=' + params.pagesize);
};
export const uploadFile = `/api/meeting/uploads`;

export const modelbd = (params) => {
    return get('/api/meeting/branchs-users?token=' + params.token);
};
export const userList = (params) => {
    return get('/api/meeting/allusers?token=' + params.token);
};

export const editMeeting = (params) => {
    return put('/api/meeting',params);
 };
 
export const getBranchs = (params) => {
    return get('/api/meeting/branchs?token=' + params.token + '&parentID=' + params.parentID);
};

export const getallUsers = (params) => {
    return get('/api/meeting/users?token=' + params.token + '&branchID=' + params.branchID);
};

export const getTextTemplate = (params) => {
    return get('/api/meeting/template?searchKey=' + params.searchKey + '&token=' + params.token);
};

export const updateTemplate = params => {
    return axios.put(`${base}/api/meeting/template`, params);
}

export const createTemplate = (params) => {
    return post('/api/meeting/template', params);
}

export const deleteTemplate = params => {
    return axios.delete(`${base}/api/meeting/template?token=` + params.token + '&templateID=' + params.value.TemplateID + '&user=' + params.user);
}

export const batchDelete = params => {
    return axios.delete(`${base}/api/meeting/template/batchDelete?token=` + params.token + '&templateIDs=' + params.templateIDs + '&user=' + params.user)
}

export const getUsers = (params) => {
    return get('/api/meeting/branchs-users?token=' + params.token)
}


export const getGroup = (params) => {
    return get('/api/meeting/group?token=' + params.token)
}

export const updateGroup = params => {
    return put('/api/meeting/group', params)
}

export const createGroup = (params) => {
    return post('/api/meeting/group', params)
}

export const batchDeleteGroup = params => {
    return axios.delete(`${base}/api/meeting/group/batchDelete?token=` + params.token + '&groupIDs=' + params.GroupIDs + '&user=' + params.user)
}

export const getLogs = (params) => {
    return get('/api/meeting/logs?token=' + params.token + '&value.Keyword=' + params.Keyword + '&value.STime=' + params.STime + '&value.ETime=' + params.ETime + '&value.BranchID=' + params.BranchID + '&value.page=' + params.page + '&value.pagesize=' + params.pagesize)
}

export const getBranchInLog = (params) => {
    return get('/api/meeting/branchs?token=' + params.token)
}