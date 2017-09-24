<template>
  <div id="cell">
    <div class="title-wrapper">
      <span class="title">{{item.MeetingName}}</span>
      <mt-badge class="title-badge" size="small" :color="a[item.MeetingStatus]">
        <span class="title-status">{{b[item.MeetingStatus]}}</span>
      </mt-badge>
    </div>
    <div class="content-wrapper">
      <div class="flex">
        <div class="flexF">会议地点：</div>
        <div class="flexS">{{item.MeetingAddr}}</div>
      </div>
      <div class="flex">
        <div class="flexF">会议时间：</div>
        <div class="flexS">{{item.StartTime}}</div>
      </div>
    </div>
  
  </div>
</template>
<script>
export default {
  props: {
    item: {
      default: {}
    }
  },
  data() {
    return {
      a: ['#000', '#000', '#5BC0DE', '#5CB85C', '#F0AD4E'],
      b: ['编辑中', '已关闭', '待进行', '进行中', '已结束'],
    }
  }
}
</script>
<style lang="scss">
#cell {
  color: black;
  text-align: left;
  background-color: #fff;
  border-top: 1px solid transparent;
  border-bottom: 1px solid transparent;
  margin-top: 21px;
  .title-wrapper {
    width: 100%;
    display: inline-flex;
    align-items: center;
    font-weight: bold;
    font-size: 18px;
    .title {
      padding: 10px 0 10px 20px;
      flex-grow: 1;
    }
    .title-badge {
      flex-shrink: 0;
      flex-grow: 0;
      width: 62px;
      height: 30px;
      font-size: 14px;
      margin: 10px;
      .title-status {
        line-height: 30px;
        color: white;
      }
    }
  }
  .content-wrapper {
    margin: 0 20px 9px 20px;
    font-size: 13px;
    .flex{
      display: flex;
      padding:5px 0;
      .flexF{
        flex-basis: 70px;
        flex-shrink:0;
      }
    }
  }
}
</style>
