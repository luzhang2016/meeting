<template>
    <div class="detail">
        <loading v-show="loading"></loading>
        <myhead title="会议资料"></myhead>
        <div class="detail-wrapper" v-for="(item,index) in details" :key="index">
            <div class="left-wrapper">
                <div class="detail-left">
                    <div class="doc-name">
                        <span class="doc-extension">{{item.FileExtension|toUpper}}</span>
                        <span class="doc-filename">{{item.FileName}}</span>
                    </div>
                    <div class="uploader">发布人：{{item.UploadUser}}</div>
                </div>
            </div>
            <div class="detail-right">
                <div class="download-times">下载次数:{{item.DownloadTimes}}次</div>
                <span class="icon" @click="toggle(index)">
                    <i class="fa fa-angle-down fa-2x" aria-hidden="true" v-show="!item.isShow"></i>
                    <i class="fa fa-angle-up fa-2x" aria-hidden="true" v-show="item.isShow"></i>
                </span>
            </div>
            <div class="detail-bottom" v-show="item.isShow">
                <div class="bottom-icon">
                    <a :href="item.Url" download @click="downFile(item, index)">
                        <i class="fa fa-download fa-lg" aria-hidden="true"></i>
                    </a>
                </div>
                <div class="bottom-icon">
                    <router-link :to="{ name: 'downloadDetail', params: {uuid:item.UUID,index:index}}">
                        <i class="fa fa-ellipsis-h fa-lg" aria-hidden="true"></i>
                    </router-link>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import user from "../user/userphone";
import { Toast } from 'mint-ui';
export default {

    data() {
        return {
            currentIndex: '',
            details: [],
            uuid: '',
            name: user.username,
            loading: true
        }
    },
    mounted() {
        this.uuid = this.$route.params.uuid;
        this.fetchData(this.uuid);
    },
    methods: {
        toggle(index) {
            this.details[index].isShow = !this.details[index].isShow;
        },
        fetchData(uuid) {
            //0附件 1记录
            this.$plugin_api.getFiles(uuid, 0).then(res => {
                this.details = res;
                for (let i = 0; i < this.details.length; i++) {
                    this.$set(this.details[i], 'isShow', false);
                }
                this.loading = false;
            })
        },
        downFile(item, index) {
            this.$plugin_api.downLoad(item.FileUUID, this.name).then(res => {
                this.details[index].DownloadTimes++;
            }).catch(err => {
                Toast("下载失败，请重试");
            });;
        }
    },
    filters: {
        toUpper(val) {
            if (!val) { return ""; }
            return val.substr(0, 3).toUpperCase();
        }
    }
}
</script>
<style scoped lang="scss">
.detail {
    margin-top: 45px;
    .detail-wrapper {
        background-color: #fff;
        margin-bottom: 12px;
        position: relative;
        border-bottom: 1px solid #bbbbbb;
        .left-wrapper {
            margin-right: 110px;
            .detail-left {
                padding: 10px 0;
                line-height: 20px;
                .doc-name {
                    margin-left: 10px;
                    height: 40px;
                    line-height: 40px;
                    white-space: nowrap;
                    overflow: hidden;
                    text-overflow: ellipsis;
                    .doc-extension {
                        display: inline-block;
                        width: 35px;
                        height: 35px;
                        line-height: 35px;
                        background: #529FFF;
                        border-radius: 5px;
                        text-align: center;
                        margin-right:10px;
                        color: white;
                        font-size: 14px;
                    }
                } 
                .uploader {
                    margin-left: 60px;
                    color: #8FA8B5;
                    font-size: 13px;
                }
            }
        }
        .detail-right {
            position: absolute;
            right: 0;
            top: 10px;
            width: 110px;
            margin: 10px 0 10px -100px;
            text-align: right;
            line-height: 20px;
            font-size: 13px;
            .download-times {
                text-align: center;
                margin-bottom: 5px;
            }
            .icon {
                padding: 0 20px 0 10px;
                i {
                    color: #bbbbbb;
                }
            }
        }
        .detail-bottom {
            display: flex;
            height: 45px;
            background: #EBEBEB;
            .bottom-icon {
                flex: 1;
                text-align: center;
                line-height: 45px;
                i {
                    color: #7A7A7A;
                }
            }
        }
    }
}
</style>
