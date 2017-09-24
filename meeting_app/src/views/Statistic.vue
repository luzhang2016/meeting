<template>
    <div id="statistic" v-cloak>
        <loading v-show="loading"></loading>
        <myhead title="数据统计"></myhead>
        <div class="number">
            <canvas id="canvas" style="position:absolute;top:0px;left:0px;z-index:1;"></canvas>
            <div class="isSign">
                <p class="isSign-bottom">
                    <span>{{data.signInTotal}}</span> 人</p>
                <p class="isSign-top">已签到</p>
            </div>
            <div class="total">总参会人数：{{data.memberTotal}} 人</div>
        </div>
        <div class="lists">
            <div class="list-th">
                <span>姓名</span>
                <span>是否签到</span>
                <span>签到时间</span>
                <span>签到排名</span>
            </div>
            <div class="list-member" v-for="(member,index) in members" :key="index">
                <span>{{member.name}}</span>
                <span :class="member.signInFlag==1?'green':'red'">
                    <i :class="map[member.signInFlag]" aria-hidden=true></i>
                </span>
                <span>{{member.signInTime.substr(11)}}</span>
                <span>{{member.signInIndex>0?member.signInIndex:''}}</span>
            </div>
        </div>
    </div>
</template>
<script>

export default {
    data() {
        return {
            uuid: '',
            members: [],
            data: {},
            map: ["fa fa-times", "fa fa-check"],
            loading: true
        }
    },
    mounted() {
        this.uuid = this.$route.params.uuid;
        this.fetchData(this.uuid);
        this.draw();
    },
    methods: {
        fetchData(uuid) {
            // let token = this.md5();
            this.$plugin_api.getSignInList(uuid).then(res => {
                this.members = res.members;
                this.data = res;
                this.loading = false;
            });
        },
        draw() {
            var canvas = document.getElementById('canvas');
            var ctx = canvas.getContext('2d');
            canvas.width = canvas.parentNode.offsetWidth;
            canvas.height = canvas.parentNode.offsetHeight;
            //填充颜色
            ctx.fillStyle = "#f00";
            //开始绘制路径
            ctx.beginPath();
            //左上角
            ctx.moveTo(0, canvas.height );
            //右上角
            ctx.lineTo(canvas.width, canvas.height / 2);
            //右下角
            ctx.lineTo(canvas.width, canvas.height);
            //左下角
            ctx.lineTo(0, canvas.height);
            //左上角
            ctx.lineTo(0, canvas.height / 2);
            //闭合路径
            ctx.closePath();
            //填充路径
            ctx.fill();
            window.requestAnimFrame = (function () {
                return window.requestAnimationFrame ||
                    window.webkitRequestAnimationFrame ||
                    window.mozRequestAnimationFrame ||
                    function (callback) {
                        window.setTimeout(callback, 1000 / 60);
                    };
            })();
            var step = 0;
            var lines = ["rgba(128,202,255, 0.5)",
                "rgba(157,192,249, 0.5)",
                "rgba(180,168,255, 0.5)"];
            function loop() {
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                step++;
                //画3个不同颜色的矩形
                for (var j = lines.length - 1; j >= 0; j--) {
                    ctx.fillStyle = lines[j];
                    //每个矩形的角度都不同，每个之间相差45度
                    var angle = (step + j * 45) * Math.PI / 180;
                    var deltaHeight = Math.sin(angle) * 30;
                    var deltaHeightRight = Math.cos(angle) * 30;
                    ctx.beginPath();
                    ctx.moveTo(0, canvas.height /2- deltaHeight);
                    ctx.bezierCurveTo(canvas.width / 2, canvas.height / 2 + deltaHeight - 50, canvas.width / 2, canvas.height / 2 + deltaHeightRight - 50, canvas.width, canvas.height / 2 + deltaHeightRight);
                    ctx.lineTo(canvas.width, canvas.height);
                    ctx.lineTo(0, canvas.height);
                    ctx.lineTo(0, canvas.height / 2 + deltaHeight);
                    ctx.closePath();
                    ctx.fill();
                }
                requestAnimFrame(loop);
            }
            loop();
        },
    },

}
</script>
<style lang="scss">
#statistic {
    .number {
        position: relative;
        margin-top: 44px;
        border-bottom: 1px solid #bbbbbb;
        background: #26A2FF;
        height:200px;;
        span {
            font-weight: bold;
            color: #26A2FF;
            font-size: 20px;
        }
        .isSign {
            position: relative;
            z-index:2;
            top:20px;
            height: 100px;
            width: 100px;
            border-radius: 50%;
            background: #fff;
            border: 8px solid #0086FF;
            margin: 0 auto 50px auto;
            text-align: center;
            font-size: 16px;
            .isSign-top {
                margin-top: 10px;
            }
            .isSign-bottom {
                margin-top: 20px;
            }
        }
        .total {
            position: absolute;
            display: inline-block;
            right:20px;
            bottom:20px;
            z-index: 5;
            color:#fff;
        }
    }
    .lists {
        background: #fff;
        border-bottom: 1px solid #bbbbbb;
        .list-th {
            display: flex;
            height: 50px;
            background: #38ADFF;
            color: #fff;
            span {
                flex: 1;
                line-height: 50px;
                text-align: center;
            }
        }
        .list-member {
            display: flex;
            height: 45px;
            line-height: 45px;
            font-size: 16px;
            span {
                flex: 1;
                text-align: center;
            }
            .red {
                color: red;
            }
            .green {
                color: green;
            }
        }
    }
}
</style>
