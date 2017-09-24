<template>
    <div id="record">
        <loading v-show="loading"></loading>
        <myhead title="会议记录"></myhead>
        <div class="record-wrapper" v-for="(item,index) in records" :key="index">
            <div class="left-wrapper">
                <div class="doc-name">
                    <span class="doc-extension">{{item.FileExtension|toUpper}}</span>
                    <span class="doc-filename">{{item.FileName}}</span>
                </div>
            </div>
            <div class="right">
                <span class="download">
                    <a :href="item.Url" download @click="downFile(item,index)" v-show="!item.isDownload">未下载</a>
                    <a v-show="item.isDownload">点击打开</a>
                </span>
            </div>
            <p class="uploader">
                发布人：{{item.UploadUser}}
                <span class="download-times">下载次数:{{item.DownloadTimes}}次</span>
            </p>
        </div>
    </div>
</template>
<script>
import user from "../user/userphone"
import { Toast } from 'mint-ui';
export default {
    data() {
        return {
            currentIndex: '',
            records: [],
            name: user.username,
            loading: true
        }
    },
    mounted() {
        this.uuid = this.$route.params.uuid;
        this.fetchData(this.uuid);
    },
    methods: {
        fetchData(uuid) {
            //0附件 1记录
            this.$plugin_api.getFiles(uuid, 1).then(res => {
                this.records = res;
                this.records.forEach(e => {
                    e.isDownload = false;
                    for (let i = 0; i < e.Download.length; i++) {
                        if (this.name == e.Download[i].downloadUser)
                            this.records[i].isDownload = true;
                    }
                });
                this.loading = false;
            })
        },
        downFile(item, index) {
            this.$plugin_api.downLoad(item.FileUUID, this.name).then(res => {
                this.records[index].DownloadTimes++;
                this.records[index].isDownload = true;
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
#record {
    margin-top: 45px;
    .record-wrapper {
        background-color: #fff;
        border-bottom: 1px solid #bbbbbb;
        .left-wrapper {
            margin-right: 75px;
            padding: 10px 0;
            line-height: 20px;
            .doc-extension {
                display: inline-block;
                width: 35px;
                height: 35px;
                line-height: 35px;
                background: #529FFF;
                border-radius: 5px;
                text-align: center;
                color: white;
                font-size: 14px;
            }
            .doc-name {
                margin-left: 10px;
                height: 40px;
                line-height: 40px;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }
        }
        .right {
            position: absolute;
            top: 58px;
            right: 10px;
            .download {
                display: inline-block;
                height: 30px;
                background: #1EB9F2;
                line-height: 30px;
                font-size: 14px;
                padding: 0 5px;
            }
            a {
                color: white;
            }
        }

        .uploader {
            position: relative;
            clear: both;
            height: 30px;
            color: #8FA8B5;
            margin-left: 10px;
            font-size: 13px;
            .download-times {
                position: absolute;
                right: 15px;
            }
        }
    }
}
</style>
