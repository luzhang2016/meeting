<template>
    <div id="registered">
        <loading v-show="loading"></loading>
        <myhead title="签到详情"></myhead>
        <p class="info">签到详情</p>
        <div class="signIn">
            <p class="order">您是本次会议第{{res.signInIndex}}个签到的用户</p>
            <p class="time">签到时间：
                <span>{{res.signInTime}}</span>
            </p>
            <p class="location">签到位置：
                <span>{{res.signInAddr}}</span>
            </p>
        </div>
        <p class="info">签到统计</p>
        <div class="chart" style="background:white;position:relative">
            <canvas id="myChart" width="200" height="100"></canvas>
            <p class="number">{{res.signInIndex}} / {{res.memberTotal}}</p>
            <p class="text">已签到/应签到</p>
            <router-link :to="'/statistic/'+this.uuid">
                <div class="data">数据统计</div>
            </router-link>
        </div>
    
    </div>
</template>
<script>
import user from "../user/userphone"
export default {
    data() {
        return {
            res: {},
            uuid: '',
            phone: user.userphone,
            loading: true
        }
    },
    mounted() {
        this.uuid = this.$route.params.uuid;
        this.fetchData(this.uuid, this.phone);
    },
    methods: {
        fetchData(uuid, phone) {
            this.$plugin_api.getSignIn(uuid, phone).then(res => {
                this.res = res;
                this.drawChart(this.res.memberTotal, this.res.signInTotal);
                this.loading = false;
            });
        },
        drawChart(a, b) {
            var canvas = document.getElementById("myChart");
            var myDoughnutChart = new Chart(canvas, {
                type: 'doughnut',
                data: {
                    labels: ['已签到', '未签到'],
                    datasets: [{
                        data: [b, a - b],
                        backgroundColor: [
                            '#26A2FF',
                            '#D4D4D4',
                        ],
                        borderWidth: 0

                    }],
                },
                options: {
                    cutoutPercentage: 85
                }
            });
        }
    }
}
</script>
<style lang="scss">
#registered {
    margin-top: 45px;
    .info {
        height: 35px;
        padding-left: 10px;
        font-size: 14px;
        line-height: 35px;
    }
    .signIn {
        font-size: 16px;
        background: #fff;
        p {
            padding-left: 10px;
            border-bottom: 1px solid #EFF2F7;
            span {
                color: #26A2FF;
            }
        }
        .order {
            height: 65px;
            line-height: 66px;
        }
        .time,
        .location {
            padding: 10px;
            line-height: 25px;
        }
    }
    .chart {
        overflow: hidden;
        .number {
            position: absolute;
            width: 50%;
            text-align: center;
            top: 33%;
            left: 25%;
            font-size: 22px;
            color: #26A2FF;
        }
        .text {
            position: absolute;
            width: 50%;
            text-align: center;
            top: 47%;
            left: 25%;
            font-size: 13px;
        }
        .data {
            margin: 20px auto;
            width: 90%;
            height: 30px;
            background: #26A2FF;
            border-radius: 8px;
            text-align: center;
            line-height: 30px;
            color: #fff;
            font-size: 14px;
        }
    }
}
</style>
