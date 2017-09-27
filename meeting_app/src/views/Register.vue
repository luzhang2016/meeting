<template>
    <div id="register">
        <myhead title="会议签到"></myhead>
        <p class="head">
            <span class="date">
                <i class="fa fa-calendar fa-lg"></i>{{date}}
            </span>
            <span class="hour">
                <i class="fa fa-clock-o fa-lg"></i>当前时间：{{nowSimple}}
            </span>
        </p>
        <div id="allmap"></div>
        <p class="info"> 当前位置：{{this.result}}</p>
        <div class="register-box" v-show="show">
            <div class="register-box-content" @click="signIn">
                <div v-show="show" class="click">
                    <div>
                        <i class="fa fa-map-marker fa-3x"></i>
                    </div>
                    签到
                </div>
            </div>
        </div>
        <transition name="fade">
            <div v-show="!show">
                <p class="head">&nbsp;&nbsp;签到详情</p>
                <div class="signIn">
                    <p class="order">您是本次会议第{{this.res.signInIndex}}个签到的用户</p>
                    <p class="time">签到时间：
                        <span>{{this.res.signInTime}}</span>
                    </p>
                    <p class="location">签到位置：
                        <span>{{result}}</span>
                    </p>
                </div>
                <p class="head">&nbsp;&nbsp;签到统计</p>
                <div class="chart" style="background:white;position:relative">
                    <canvas id="myChart" width="200" height="100"></canvas>
                    <p class="number">{{res.signInIndex}} / {{res.memberTotal}}</p>
                    <p class="text">已签到/应签到</p>
                    <router-link :to="'/statistic/'+this.mid" replace>
                        <div class="data">数据统计</div>
                    </router-link>
                </div>
            </div>
        </transition>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import user from "../user/userphone"
export default {
    data() {
        return {
            now: this.getTime(new Date()),
            show: true,
            count: 1,
            result: '',
            uuid: '',
            res: {},
            phone: user.userphone,
        }
    },
    computed: {
        nowSimple: function () {
            let str = this.getTime(new Date());
            return str.slice(11, 16);
        },
        date:function(){
            let str = new Date().toLocaleDateString().split("/");
            return str[0]+'年'+str[1]+'月'+str[2]+'日';
        }
    },
    methods: {
        signIn() {
            if (!this.result) {
                Toast("定位中，请稍候");
                return;
            }
            this.$plugin_api.SignIn(this.mid, this.phone, this.result).then(res => {
                this.res = res;
               Toast("签到成功");
                this.drawChart(this.res.memberTotal, this.res.signInIndex);
                this.show = false;
            }).catch(err => {
                Toast("签到失败，请重试");
            });;
        },
        getTime(e) {
            let month = (e.getMonth() + 1) < 10 ? "0" + (e.getMonth() + 1) : (e.getMonth() + 1);
            let day = e.getDate() < 10 ? "0" + e.getDate() : e.getDate();
            let h = e.getHours() < 10 ? "0" + e.getHours() : e.getHours();
            let min = e.getMinutes() < 10 ? "0" + e.getMinutes() : e.getMinutes();
            let s = e.getSeconds() < 10 ? "0" + e.getSeconds() : e.getSeconds();
            return e.getFullYear() + '-' + month + '-' + day + ' ' + h + ":" + min + ":" + s;
        },
        drawChart(a, b) {
            var ctx = document.getElementById("myChart");
            var myPieChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ["已签到", "未签到"],
                    datasets: [{
                        data: [b, a - b],
                        backgroundColor: [
                            '#26A2FF',
                            '#D4D4D4'
                        ],
                        borderWidth: 0
                    }],
                },
                options: {
                    cutoutPercentage: 85
                }
            });
        }
    },
    mounted() {
        this.mid = this.$route.params.mid;
        let map = new BMap.Map('allmap');
        let point = new BMap.Point(116.404, 39.915);
        map.centerAndZoom(point, 18);
        map.addControl(new BMap.NavigationControl());
        map.setCurrentCity("杭州");
        map.enableScrollWheelZoom(true);
        map.enableDoubleClickZoom(true);
        // 定位对象
        var geolocation = new BMap.Geolocation();
        let vm = this;
        geolocation.getCurrentPosition(function (r) {
            if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                let mk = new BMap.Marker(r.point);
                map.addOverlay(mk);
                map.panTo(r.point);
                let geoc = new BMap.Geocoder();
                geoc.getLocation(r.point, function (rs) {
                    let addComp = rs.addressComponents;
                    vm.result = addComp.city + addComp.district + addComp.street + addComp.streetNumber;
                });
            } else {
                alert('无法定位到您的当前位置！');
            }
        }, { enableHighAccuracy: true });
    },
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

#register {
    margin-top: 45px;
    .head {
        height: 40px;
        line-height: 40px;
        span {
            height: 40px;
            margin-right: 10px;
            font-size: 14px;
            i {
                margin: 10px;
                color: #38ADFF;
            }
        }
        .hour {
            float: right;
        }
    }
    #allmap {
        width: 100%;
        height: 35vh;
    }
    .info {
        padding: 10px 10px 10px 16px;
        line-height: 25px;
        background: #fff;
        font-size: 18px;
        border-bottom: 1px solid #ddd;
    }
    .register-box {
        border: 1px solid #ddd;
        background: #38ADFF;
        height: 141px;
        width: 141px;
        margin: 23px auto;
        border-radius: 50%;
        text-align: center;
        font-size: 15px;
        .register-box-content {
            i {
                color: #fff;
                padding-bottom: 10px;
            }
            .click {
                margin-top: 25px;
                font-size: 18px;
                color: #fff;
            }
            .isClick {
                margin-top: 55px;
                color: #fff;
                font-weight: bold;
                font-size: 20px;
            }
        }
        .register-detail {
            margin-top: 20px;
            p {
                font-size: 13px;
                margin: 5px 20px;
            }
        }
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
            height: 55px;
            line-height: 55px;
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
