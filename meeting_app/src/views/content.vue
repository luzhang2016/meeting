<template>
    <div id="content">
        <myhead title="会议内容"></myhead>
        <div class="content-wrapper">
            <p class="title">会议内容:</p>
            <p class="artical">{{content}}</p>
        </div>
    </div>
</template>
<script>
export default {

    data() {
        return {
            content: '',
            uuid: ''
        }
    },
    mounted() {
        // var reg = /\/meeting\/(\d+)/;
        // var id = this.$route.path.match(reg)[1];
        this.uuid = this.$route.params.uuid;
        this.fetchData(this.uuid);
    },
    methods: {
        fetchData(uuid) {
            this.$plugin_api.getMeeting(uuid).then(res => {
                this.content = res.MeetingContent;
            });
        }
    },
}
</script>

<style scoped lang="scss">
#content {
    .content-wrapper {
        padding-top: 45px;
        font-size: 16px;
        background: white;
        // border-top: 1px solid #8492A6;
        // border-bottom: 1px solid #8492A6;
        .title {
            padding:20px;
            border-bottom:1px solid #EFF2F7;
        }
        .artical {
            text-indent: 2em;
            padding:20px;
            line-height: 25px;
        }
    }
}
</style>