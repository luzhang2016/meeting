<template>
  <div id="member">
    <loading v-show="loading"></loading>
    <div class="top-fixed">
      <myhead title="会议成员"></myhead>
      <mt-search v-model="value" style="height:100%;" cancel-text="取消" placeholder="搜索">
        <div class="mint-search-list"></div>
      </mt-search>
    </div>
    <div class="member-list">
      <div v-show="this.host.length">
        <p class="firstP">主持人</p>
        <memberCell v-for="(item,index) in host" :member='item' :key="index"></memberCell>
      </div>
      <div v-show="this.speaker.length">
        <p>发言人</p>
        <memberCell v-for="(item,index) in speaker" :member='item' :key="index"></memberCell>
      </div>
      <div v-show="this.audience.length">
        <p>参会人员</p>
        <memberCell v-for="(item,index) in audience" :member='item' :key="index"></memberCell>
      </div>
    </div>
  </div>
</template>
<script>
import memberCell from "../components/member-cell"
export default {
  components: {
    memberCell
  },
  data() {
    return {
      value: '',
      host: [],
      speaker: [],
      audience: [],
      MeetingMember: [],
      loading: true
    }
  },
  mounted() {
    this.mid = this.$route.params.mid;
    this.fetchData(this.mid);
  },
  watch: {
    value: function () {
      let list = this.searchByRegExp(this.value, this.MeetingMember);
      this.host = [];
      this.speaker = [];
      this.audience = [];
      this.fleterByFlag(list);
    }
  },
  methods: {
    fetchData(mid) {
      this.$plugin_api.getMember(mid).then(res => {
        this.MeetingMember = res;
        this.fleterByFlag(this.MeetingMember);
        this.loading = false;
      });
    },
    searchByRegExp(keyword, list) {
      if (!(list instanceof Array)) {
        return;
      }
      var arr = [];
      var reg = new RegExp(keyword);
      for (let i = 0; i < list.length; i++) {
        if (list[i].name.match(reg)) {
          arr.push(list[i]);
        }
      }
      return arr;
    },
    fleterByFlag(list) {
      list.forEach(e => {
        if (e.Flag == 1) this.host.push(e);
        if (e.Flag == 2) this.speaker.push(e);
        if (e.Flag == 3) this.audience.push(e);
      })
    }
  }
}
</script>

<style lang="scss">
#member {
  .top-fixed {
    position: fixed;
    top: 45px;
    width: 100%;
    z-index: 50;
    .mint-search {
      .mint-search-list {
        z-index: -100;
      }
    }
  }
  .member-list {
    p {
      font-size: 15px;
      font-weight: bold;
      padding:10px;
    }
    margin-top: 98px;
    .firstP {
    }
  }
}
</style>