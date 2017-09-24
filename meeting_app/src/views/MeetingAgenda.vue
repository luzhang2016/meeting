<template>
  <div class="meetingAgenda">
    <loading v-show="loading"></loading>
    <myhead title="会议议程"></myhead>
    <div class="agenda-wrapper">
      <p class="title-wrapper">会议议程：{{MeetingAgenda.title}}</p>
      <p class="nameTime">
        <span>发言人：{{MeetingAgenda.name}}</span>
        <span class="time">时间：{{MeetingAgenda.sTime|clock}}-{{MeetingAgenda.eTime|clock}}</span>
      </p>
      <p class="content">{{MeetingAgenda.content}}</p>
    </div>
    <div>
  
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      uuid: '',
      index: '',
      MeetingAgenda: '',
      loading:true
    }
  },
  mounted() {
    this.uuid = this.$route.params.uuid;
    this.index = this.$route.params.index;
    this.fetchData(this.uuid, this.index);
  },
  methods: {
    fetchData(uuid, index) {
      
      this.$plugin_api.getMeeting(uuid).then(res => {
        this.MeetingAgenda = res.MeetingAgenda[index];
        this.loading=false;
      }
      )
    }
  },
  filters: {
    clock(time) {
      if(!time) return;
      return time.substr(11, 5);
    }
  }
}
</script>
<style lang="scss">
.meetingAgenda {
  .agenda-wrapper {
    margin-top: 45px;
    background: #fff;
    font-size: 16px;
    .title-wrapper {
      padding: 15px 25px;
      font-weight: bold;
      font-size:18px;
      .title {
        margin-left: 12px;
      }
    }
    .nameTime {
      padding:0 0 15px 25px;
      position: relative;
      color: #8FA8B5;
      border-bottom:1px solid #EFF2F7;
      .time {
        margin-left:35px;
      }
    }
    .content {
      text-indent: 2em;
      padding: 20px 25px;
      line-height: 25px;
    }
  }
}
</style>
