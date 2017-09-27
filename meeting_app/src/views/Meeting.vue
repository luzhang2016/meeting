<template>
  <div id="meeting">
    <!--<loading v-show="loading"></loading>-->
    <div class="top">
      <myhead title="会议详情">
        <span slot="right" class="right">
          <router-link :to="'/register/'+this.mid" v-show="flag==0">
            <span class="right-name">签到</span>
          </router-link>
          <router-link :to="'/registered/'+this.mid" v-show="flag==1">
            <span class="right-name">签到详情</span>
          </router-link>
        </span>
      </myhead>
    </div>
    <div class="main">
      <div class="meeting-review">
        <p class="meeting-name">{{meetingData.MeetingName}}</p>
        <p>
          <span class="icon">
            <i class="fa fa-map-marker fa-fw" aria-hidden="true"></i>
          </span>会议地点：{{meetingData.MeetingAddr}}</p>
        <p class="border">
          <span class="icon">
            <i class="fa fa-clock-o fa-fw" aria-hidden="true"></i>
          </span>会议时间：{{meetingData.StartTime}}</p>
        <p>
          <span class="icon">
            <i class="fa fa-sticky-note-o fa-fw" aria-hidden="true"></i>
          </span>会议摘要：{{meetingData.MeetingSummary}}
        </p>
      </div>
      <div class="meeting-detail">
        <p class="meetingProcess">会议议程</p>
        <timeline :meetingData="this.meetingData"></timeline>
      </div>
      <extra :data="this.meetingData.MeetingBusiness"></extra>
    </div>
    <myFooter :mid="this.mid"></myFooter>
  </div>
</template>

<script>
import myFooter from '@/components/footer'
import timeline from "../components/timeline"
import extra from "../components/meetingExtra"
import user from "../user/userphone"
export default {
  components: {
    myFooter,
    timeline,
    extra
  },
  mounted() {
    this.mid = this.$route.params.mid;
    // console.log(this.$route.params.mid)
    this.fetchData(this.mid,this.phone);
  },
  methods: {
    fetchData(mid,phone) {
        this.$plugin_api.getMeeting(mid,phone).then(res => {
          console.log(res)
        this.members = res.MeetingMember;
        this.meetingData = res[0];
        this.flag=res[0].SignInFlag
        // this.loading = false;
      })
    }
  },
  data() {
    return {
      mid: null,
      meetingData: {},
      flag: '2',
      load: true,
      phone: 13806619662,
      loading: true
    }
  },
}
</script>

<style scoped lang="scss">
#meeting {
  font-size: 12px;
  .top {
    .right {
      width: 88px;
      height: 30px;
      border-radius: 4px;
      .right-name {
        color: white;
        width: 88px;
        height: 30px;
        line-height: 30px;
      }
    }
  }
  .main {
    padding: 45px 0 0px 0;
    .meeting-review {
      background: white;
      font-size: 16px;
      .meeting-name {
        font-size: 18px;
        font-weight: 600;
      }
      p {
        padding: 10px;
        &.border {
          border-top: 1px solid #E5E9F2;
          border-bottom: 1px solid #E5E9F2;
        }
        .icon {
          color: #8492A6;
          margin-right: 5px;
        }
      }
    }
    .meeting-detail {
      background: white;
      margin-top: 25px;
      margin-bottom: 25px;
      border-bottom: 1px solid transparent;
      .meetingProcess {
        height: 45px;
        padding: 4px 0 6px 17px;
        line-height: 45px;
        background: white;
        font-size: 16px;
        font-weight: bold;
        color: #26A2FF;
      }
      .meetingContent {
        font-size: 16px;
        .meetingTime {
          height: 40px;
          background-color: transparent;
          border-top: 1px solid #8492A6;
          border-bottom: 1px solid #8492A6;
          line-height: 40px;
          font-size: 16px;
          .time-icon {
            padding: 0 14px 0 14px;
          }
        }
        .meeting {
          background: #fff;
          padding-left: 19px;
          .meeting-title {
            width: 90%;
            height: 42px;
            line-height: 42px;
            border-bottom: 1px solid #8492A6;
          }
          .meeting-speaker {
            height: 45px;
            line-height: 45px;
          }
          .meeting-content {
            position: relative;
            padding: 0 67px 13px 0;
            .icon {
              position: absolute;
              right: 20px;
              top: 0px;
            }
          }
        }
      }
    }
  }
}
</style>
