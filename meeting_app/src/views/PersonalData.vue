<template>
    <div class="personalData">
        <mt-header fixed title="详细资料">
            <a slot="left" onclick="window.history.go(-1)">
                <mt-button icon="back">返回</mt-button>
            </a>
            <span slot="right">消息记录</span>
        </mt-header>
        <div class="header">
            <div class="avatar">
                <img height="50" width="50" :src="person.avatar">
            </div>
            <div class="content">
                <div class="name">{{person.name}}</div>
                <div class="signature">{{person.signature}}</div>
            </div>
        </div>
        <div class="main">
            <div class="main-cell">单位：{{person.company}}</div>
            <hr>
            <div class="main-cell">部门：{{person.department}}</div>
            <hr>
            <div class="main-cell">职务：{{person.job}}</div>
            <hr>
            <div class="main-cell">电话：{{person.tel}}</div>
            <hr>
            <div class="main-cell">手机：{{person.mob}}</div>
            <hr>
            <div class="main-cell">短号：{{person.short}}</div>
            <hr>
            <div class="main-cell">邮箱：{{person.mail}}</div>
        </div>
        <div class="switch-wrapper">
            <div class="switch-title">
                <span class="switch-text">特别关心</span>
            </div>
            <div class="switch-value">
                <mt-switch v-model="value"></mt-switch>
            </div>
        </div>
        <p class="switch-intro">开启后，好友将收藏到[特别关心]列表</p>
    
    </div>
</template>
<script>
export default {
    mounted() {
        this.fetchData();
    },
    methods: {
        fetchData() {
            this.$http.get('http://127.0.0.1:3000/member').then(res=>{
                this.person = res.data.host[0];
            });
        }
    },
    data() {
        return {
            person: [],
            value: false
        };
    },
}

</script>
<style >
.personalData {
    font-size: 10px;
    margin-top: 50px;
}

.personalData .header {
    background: white;
    font-size: 0;
    padding: 12px 12px 12px 12px;
    border-top: 1px solid #8492A6;
    border-bottom: 1px solid #8492A6;
}

.personalData .avatar {
    display: inline-block;
}

.personalData .content {
    width: 200px;
    display: inline-block;
    font-size: 14px;
    margin-left: 16px;
}

.personalData .content .name {
    font-weight: bold;
}

.personalData .content .signature {
    font-size: 14px;
    padding-top: 5px;
}

.personalData .main {
    margin: 15px 0;
    border-top: 1px solid #8492A6;
    border-bottom: 1px solid #8492A6;
    background: white;
}

.personalData .main .main-cell {
    padding: 10px 20px;
    font-size: 14px;
}

hr {
    height: 1px;
    border: none;
    border-top: 1px solid #8492A6;
    width: 90%;
    margin: 0px auto;
}

.personalData .switch-wrapper {
    padding: 10px 20px;
    background: white;
    display: flex;
    align-items: center;
}

.personalData .switch-wrapper .switch-title {
    font-size: 14px;
    flex: 1;
}

.switch-intro {
    color: #8492A6;
    padding-left: 20px;
    margin: 0px;
}
</style>