<template>
  <div class="app">
    <loading v-show="loading"></loading>
    <div class="top-fixed">
      <myhead title="会议日程">
        <span slot="left" class="left">首页</span>
        <span slot="right" class="right" @click="toggleFilter">
          <i class="fa fa-calendar fa-lg" aria-hidden="true"></i>
        </span>
      </myhead>
      <div class="select-wrapper">
        <div class="select" v-for="(item,index) in tabs" :key="index" :class='{active:activeName == item}' @click='keyShowFun(item,index)'>
          <span>{{item}}</span>
        </div>
      </div>
    </div>
    <div class="content">
      <router-link :to="'/meeting/'+item.UUID" v-for="(item,index) in selection " :key="index">
        <cell :item="item"></cell>
      </router-link>
    </div>
    <transition name="fade">
      <div class="filter-wrapper" v-show="filterShow">
        <div class="filter">
          <p class="filter-title">选择查看时间</p>
          <div class="filter-main">
            <a v-for="(time,index) in filterTime" :key="index" :class='{active:activeTime == time}' @click="selectTime(time,index)">{{time}}</a>
          </div>
          <div class="filter-bottom">
            <span class="cancer" @click="toggleFilter">取消</span>
            <span class="confirm" @click="confirm">确定</span>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import cell from "../components/cell"
export default {
  name: 'app',
  components: {
    cell,
  },
  mounted() {
    //读取缓存
    var str = sessionStorage.obj;
    if (!str) {
      //无缓存即默认请求前后一个月数据
      this.start = this.transfer(this.sTime);
      this.end = this.transfer(this.eTime);
      this.fetchData(this.start, this.end);
    } else {
      //将缓存转换为对象 
      this.selection = JSON.parse(str);
      this.save = JSON.parse(str);
      this.loading = false;
    }
  },
  methods: {
    fetchData(sTime, eTime) {
      // let token = this.md5();
      this.selection = [];
      this.$plugin_api.getListSimple(sTime, eTime).then(res => {
        res.forEach(e => {
          if (e.MeetingStatus >= 2) { this.selection.push(e) }
        });
        this.loading = false;
        this.save = this.selection;
        //存入
        sessionStorage.obj = JSON.stringify(this.selection);
      });
    },
    keyShowFun(item, index) {
      this.activeName = item;
      switch (index) {
        case 1://进行中
          this.selection = [];
          this.save.forEach(e => {
            if (e.MeetingStatus == 3) { this.selection.push(e) };
          })
          break;
        case 2://待进行
          this.selection = [];
          this.save.forEach(e => {
            if (e.MeetingStatus == 2) { this.selection.push(e) };
          })
          break;
        case 3: //已结束
          this.selection = [];
          this.save.forEach(e => {
            if (e.MeetingStatus == 4) { this.selection.push(e) };
          })
          break;
        default://全部
          if (index == 0) this.selection = this.save;
      }
  },
  toggleFilter() {
    this.filterShow = !this.filterShow;
  },
  selectTime(time, index) {
    this.activeTime = time;
    this.index = index;
  },
  confirm() {
    if (this.index == 0) {
      //前后一个月
      this.start = this.transfer(this.sTime);
      this.end = this.transfer(this.eTime);
    } else if (this.index == 1) {
      //今天
      this.start = this.transfer(this.sTime + 30 * 24 * 60 * 60 * 1000);
      this.end = this.transfer(this.eTime - 30 * 24 * 60 * 60 * 1000);
    } else if (this.index == 2) {
      //前后3个月
      this.start = this.transfer(this.sTime - 30 * 24 * 60 * 60 * 1000 * 2);
      this.end = this.transfer(this.eTime + 30 * 24 * 60 * 60 * 1000 * 2);
    } else {
      //前后一年
      this.start = this.transfer(this.sTime - 30 * 24 * 60 * 60 * 1000 * 11);
      this.end = this.transfer(this.eTime + 30 * 24 * 60 * 60 * 1000 * 11);
    }
    this.fetchData(this.start, this.end)
    this.toggleFilter();
  },
  transfer(time) {
    let e = new Date(time);
    let month = (e.getMonth() + 1) < 10 ? "0" + (e.getMonth() + 1) : (e.getMonth() + 1);
    let day = e.getDate() < 10 ? "0" + e.getDate() : e.getDate();
    return e.getFullYear() + '-' + month + '-' + day;
  }
},
data() {
  return {
    save: [],
    loading: true,
    selection: [],
    filterShow: false,
    activeName: '全部',
    index: 0,
    sTime: new Date().getTime() - 30 * 24 * 60 * 60 * 1000,
    eTime: new Date().getTime() + 30 * 24 * 60 * 60 * 1000,
    start: '',
    end: '',
    activeTime: '前后一个月',
    tabs: ['全部', '进行中', '待进行', '已结束',],
    filterTime: ['前后一个月', '今天', '前后三个月', '三个月以前']
  };
}
}
</script>

<style lang="scss">
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s ease-out;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.app {
  .top-fixed {
    position: fixed;
    top: 45px;
    width: 100%;
    z-index: 50;
    .select-wrapper {
      display: flex;
      height: 54px;
      background: white;
      border-bottom: 1px solid #bbbbbb;
      font-size: 16px;
      color: #8492A6;
      .select {
        display: inline-block;
        flex: 1;
        text-align: center;
        height: 44px;
        padding: 5px 0;
        span {
          line-height: 45px;
        }
      }
      .active {
        border-bottom: 3px solid #26A2FF;
        color: #26A2FF;
      }
    }
  }
  .content {
    margin-top: 120px;
  }
  .filter-wrapper {
    width: 100%;
    height: 100%;
    position: fixed;
    left: 0;
    top: 0;
    z-index: 50;
    background: rgba(240, 240, 242, 0.5);
    .filter {
      position: relative;
      width: 284px;
      height: 160px;
      position: fixed;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      border: 1px solid #BBBBBB;
      border-radius: 5px;
      background: white;
      font-size: 0;
      .filter-title {
        height: 40px;
        line-height: 40px;
        text-align: center;
        font-weight: bold;
        font-size: 16px;
      }
      .filter-main {
        margin: 0 28px;
        a {
          display: inline-block;
          margin-top: 9px;
          height: 19px;
          width: 74px;
          text-align: center;
          line-height: 21px;
          border: 1px solid #BBBBBB;
          font-size: 12px;
        }
        .active {
          color: white;
          background: #FF7043;
        }
      }
      .filter-bottom {
        position: absolute;
        bottom: 0;
        width: 100%;
        height: 45px;
        span {
          display: inline-block;
          width: 140px;
          text-align: center;
          line-height: 43px;
          border: 1px solid #BBBBBB;
          font-size: 16px;
          font-weight: bold;
          &.confirm {
            color: #1C74D9;
          }
          &.cancer {
            color: #888888;
          }
        }
      }
    }
  }
}

:link {
  text-decoration: none;
}
</style>
