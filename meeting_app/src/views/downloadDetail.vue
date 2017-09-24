<template>
  <div class="downloadDetail">
    <loading v-show="loading"></loading>
    <myhead title="会议资料"></myhead>
    <div class="main-wrapper">
      <p class="extension">
        <span>{{data.FileExtension|toUpper}}</span>
      </p>
      <p class="filename">{{data.FileName}}.{{data.FileExtension}}
      </p>
      <p class="length">{{data.FileLength|bytesToSize}}</p>
      <p class="upload">
        <span>发布人：{{data.UploadUser}}</span>
        <span style="float:right">发布时间：{{data.UploadTime}}</span>
      </p>
      <p class="downTimes">总下载次数：{{data.DownloadTimes}} 次</p>
    </div>
    <div class="list-wrapper">
      <p class="list-title">下载记录</p>
      <div class="list-member" v-for="(item,index) in data.Download" :key="index">
        <span>{{item.downloadUser}}</span>
        <span class="downloadTime">{{item.downloadTime}}</span>
      </div>
    </div>
  </div>
</template>
<script>
export default {

  data() {
    return {
      data: '',
      loading: true
    }
  },
  mounted() {
    this.uuid = this.$route.params.uuid;
    this.index = this.$route.params.index;
    this.fetchData(this.uuid, this.index);
  },
  methods: {
    fetchData(uuid, index) {
      this.$plugin_api.getFiles(uuid, 0).then(res => {
        this.data = res[index];
        this.loading = false;
      }
      )
    }
  },
  filters: {
    bytesToSize(bytes) {
      if (bytes === 0) return '0 B';
      var k = 1024, // or 1024
        sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
        i = Math.floor(Math.log(bytes) / Math.log(k));
      return (bytes / Math.pow(k, i)).toPrecision(3) + ' ' + sizes[i];
    },
    toUpper(val) {
      if (!val) { return ""; }
      return val.substr(0, 3).toUpperCase();
    }
  }
}
</script>
<style lang="scss">
.downloadDetail {
  margin-top: 45px;
  .main-wrapper {
    background: white;
    font-size: 13px; 
    .extension {
      text-align: center;
      padding-top: 35px;
      span {
        display: inline-block;
        width: 75px;
        height: 75px;
        line-height: 75px;
        background: #33B670;
        color: white;
        text-align: center;
        font-size: 26px;
      }
    }
    .filename {
      text-align: center;
      font-size: 18px;
      padding:10px 20px 0 20px;
    }
    .length {
      text-align: center;
      color: #8FA8B5;
      padding-top: 10px;
    }
    .upload {
      padding:15px;
    }
    .downTimes {
      padding:0 0 15px 15px;
    }
  }
  .list-wrapper {
    margin-top: 30px;
    background: white;
    .list-title {
      padding-left: 20px;
      height: 45px;
      line-height: 45px;
      border-bottom:1px solid #E5E9F2;
    }
    .list-member {
      span {
        line-height: 45px;
        text-align: center;
        padding-left:30px;
      }
      .downloadTime {
        font-size: 13px;
        color:#8FA8B5;
        float:right;
        padding-right:30px;
      }
    }
  }
}
</style>
