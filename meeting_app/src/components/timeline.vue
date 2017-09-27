<template>
	<div class="container">
		<ul class="cbp_tmtimeline">
			<li v-for="(item,index) in meetingData.MeetingAgenda" :key="index">
				<time class="cbp_tmtime">
					<span class="date">{{item.STime|date}}</span>
					<span class="time">{{item.STime|clock}}</span>
					<span class="day">{{item.STime|day}}</span>
				</time>
				<div class="cbp_tmicon"></div>
				<div class="cbp_tmlabel">
					<p class="title">{{item.Title}}</p>
					<p class="speaker">发言人：{{item.Name}}</p>
					<p class="content"> 内容摘要：{{item.Summary}}</p>
					<router-link :to="{ name: 'agenda', params: { mid: meetingData.mid,index:index}}">
						<p class="more">详情
							<span>
								<i class="fa fa-angle-right fa-lg" aria-hidden="true"></i>
							</span>
						</p>
					</router-link>
				</div>
			</li>
		</ul>
	</div>
</template>
<script>
export default {
	props: {
		meetingData: '',
	},
	filters: {
		clock(time) {
			return time.substr(11, 5);
		},
		date(time) {
			let arys1 = time.split('-');
			let month = parseInt(arys1[1])
			let day = parseInt(arys1[2])
			return month + "月" + day + "日" 
		},
		day(time){
			let arys1 = time.split('-');
			let month = parseInt(arys1[1])
			let day = parseInt(arys1[2])
			let ssdate = new Date(arys1[0], month - 1, day);
			let week = String(ssdate.getDay()).replace("0", "日").replace("1", "一").replace("2", "二").replace("3", "三").replace("4", "四").replace("5", "五").replace("6", "六")//就是你要的星期几
			return "星期"+week; //就是你要的星期几
		}

	}
}
</script>

<style lang="scss">
.cbp_tmtimeline {
	list-style: none;
	position: relative;
	background: white;
	/* The line */
	&:before {
		content: '';
		position: absolute;
		top: 18px;
		bottom: 0;
		width: 1px;
		background: #20A0FF;
		left: 87px;
	}
	>li {
		position: relative;
		/* The date/time */
		.cbp_tmtime {
			display: block;
			padding-right: 100px;
			position: absolute;
			span {
				padding-left: 15px;
				display: block;
				text-align: center;
				&.date {
					font-size: 14px;
					color: #bdd0db;
				}
				&.time {
					font-size: 18px;
					color: #3594cb;
				}
				&.day {
					font-size: 14px;
					color: #bdd0db;
				}
			}
		}
		/* Right content */
		.cbp_tmlabel {
			margin: 0 15px 25px 110px;
			background: #F5FAFD;
			border: 1px solid #26A2FF;
			color: #000;
			font-weight: 300;
			position: relative;
			border-radius: 10px;
			/* The triangle */
			&:before {
				right: 100%;
				border: solid transparent;
				content: " ";
				height: 0;
				width: 0;
				position: absolute;
				pointer-events: none;
				border-right-color: #F5FAFD;
				border-width: 10px;
				top: 12px;
				left: -19px;
				z-index: 1;
			}
			&:after {
				right: 100%;
				border: solid transparent;
				content: " ";
				height: 0;
				width: 0;
				position: absolute;
				pointer-events: none;
				border-right-color: #26A2FF;
				border-width: 11px;
				top: 11px;
			}
			.title {
				padding: 5px 0 5px 0px;
				font-size: 15px;
				color: #26A2FF;
			}
			.speaker {
				font-size: 15px;
			}
			p {
				font-size: 15px;
				margin: 5px 10px 5px 10px;
				&.content {
					position: relative;
					padding-top: 5px;
					padding-bottom: 5px;
					line-height: 19px;
					max-height: 55px;
					overflow: hidden;
				}
			}
			.more {
				font-size: 14px;
				/*position: relative;*/
				margin: 0 10px 8px 10px;
				color: #8492A6;
				padding-top: 5px;
				border-top: 1px solid rgba(153, 169, 191, 0.5);
				span {
					float: right;
				}
			}
		}
		/* The icons */
		.cbp_tmicon {
			width: 10px;
			height: 10px;
			font-size: 16px;
			line-height: 40px;
			-webkit-font-smoothing: antialiased;
			position: absolute;
			color: #fff;
			background: white;
			border-radius: 50%;
			box-shadow: 0 0 0 2px #20A0FF;
			text-align: center;
			left: 107px;
			top: 0;
			margin: 18px 0 0 -25px;
		}
	}
}

</style>

