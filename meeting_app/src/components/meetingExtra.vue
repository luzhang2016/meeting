<template>
    <div class="extra" v-if="this.data">
        <div class="wrapper">
            <p class="list-title" v-if="data[0].rAddress&&data[0].rTell&&data[0].rTime">
                <b>会务组信息</b>
            </p>
            <div class="list-member" v-if="data[0].rAddress">
                <span>会务组地址</span>
                <span class="downloadTime">{{data[0].rAddress}}</span>
            </div>
            <div class="list-member" v-if="data[0].rTell">
                <span>会务组电话</span>
                <span class="downloadTime">{{data[0].rTell}}</span>
            </div>
            <div class="list-member" v-if="data[0].rTime">
                <span>接待时间</span>
                <span class="downloadTime">{{data[0].rTime}}</span>
            </div>
        </div>
        <div class="wrapper" v-if="data[0].schedule">
            <p class="list-title">
                <b>作息安排</b>
            </p>
            <div class="list-th">
                <span>时间</span>
                <span>内容</span>
            </div>
            <div class="list-member" v-for="(item,index) in data[0].schedule" :key="index">
                <span>{{item.sTime}}-{{item.eTime}}</span>
                <span class="downloadTime">{{item.item}}</span>
            </div>
        </div>
        <div class="wrapper" v-if="data[0].pickup">
            <p class="list-title">
                <b>接送安排</b>
            </p>
            <div class="list-th">
                <span>时间</span>
                <span>内容</span>
            </div>
            <div class="list-member" v-for="(item,index) in data[0].pickup" :key="index">
                <span>{{item.sTime}}-{{item.eTime}}</span>
                <span class="downloadTime">{{item.item}}</span>
            </div>
        </div>
        <div class="wrapper" v-if="data[0].meetingItems">
            <p class="list-title">
                <b>会议物品</b>
            </p>
            <div class="list-member">
                <span class="downloadTime">{{data[0].meetingItems}}</span>
            </div>
        </div>
        <div class="wrapper" v-if="data[0].matters">
            <p class="list-title">
                <b>注意事项</b>
            </p>
            <div class="list-member">
                <span class="downloadTime">{{data[0].matters}}</span>
            </div>
        </div>
    
    </div>
</template>
<script>
export default {
    props: {
        data: {
            type: Array,
            default: function () {
                return ['']
            }
        }
    }
}
</script>
<style lang="scss">
.extra {
    // background: #fff;
    font-size: 16px;
    padding-bottom: 100px;
    .wrapper {
        background: #fff;
        margin-bottom: 25px;
        .list-title {
            padding-left: 15px;
            color: #26A2FF;
            height: 45px;
            line-height: 45px;
        }
        .list-th {
            display: flex; // margin-right: 30px;
            margin-bottom: 10px;
            span {
                flex: 1;
                height: 30px;
                line-height: 30px;
                text-align: center;
            }
        }
        .list-member {
            display: flex;
            min-height: 30px; // margin-right: 30px;
            padding-bottom: 10px;
            color: #8492A6;
            align-items: center;
            span {
                flex: 1;
                text-align: center;
                padding: 0 10px;
            }
        }
    }
}
</style>
