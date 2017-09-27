<template>
  <div id="tabbar">
    <div class="item">
      <router-link :to="'/detail/'+this.mid">
        <div class="icon">
          <i class="fa fa-folder-open-o fa-2x" aria-hidden="true"></i>
        </div>
        <div class="item-name">
          会议资料
        </div>
      </router-link>
    </div>
    <div class="item">
      <router-link :to="'/content/'+this.mid">
        <div class="icon">
          <i class="fa fa-clipboard fa-2x" aria-hidden="true"></i>
        </div>
        <div class="item-name">
         会议内容
        </div>
      </router-link>
    </div>
    <div class="item">
      <router-link :to="'/member/'+this.mid">
        <div class="icon">
          <i class="fa fa-user-o fa-2x" aria-hidden="true"></i>
        </div>
        <div class="item-name">
          会议成员
        </div>
      </router-link>
    </div>
    <div class="item">
      <router-link :to="'/record/'+this.mid">
        <div class="icon">
          <i class="fa fa-pencil-square-o fa-2x" aria-hidden="true"></i>
        </div>
        <div class="item-name">
          会议记录
        </div>
      </router-link>
    </div>
  </div>
</template>
<script>
export default {
props:{
  mid:{
    type:String,
    default:''
  }
}
}
</script>

<style lang="scss">
#tabbar {
  position: fixed;
  bottom: 0;
  left: 0;
  width: 100%;
  display: flex;
  height: 87px;
  background-color: #f8f8f8;
  border-top:1px solid #8492A6;
  z-index: 50;
  .item {
    flex: 1;
    .icon {
      text-align: center;
      color: #26A2FF;
      font-size: 14px;
      height: 45px;
      i{
        padding-top:15px;
      }
    }
    .item-name {
      text-align: center;
      color: #26A2FF;
      font-size: 14px;
      height:32px;
      line-height:32px;
    }
  }
}
</style>
