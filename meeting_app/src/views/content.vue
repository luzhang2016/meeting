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
            mid: ''
        }
    },
    mounted() {
        this.mid = this.$route.params.mid;
        this.fetchData(this.mid);
    },
    methods: {
        fetchData(mid) {
            this.$plugin_api.getMeeting(mid).then(res => {
                this.content = res[0].MeetingContent;
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