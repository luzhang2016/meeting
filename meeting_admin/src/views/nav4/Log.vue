
<template>
	<div id="log">
		<!--上部工具条-->
		<el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
			<el-form :inline="true">
				<el-form-item>
					查询方式：
					<el-select v-model="model">
						<el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value"></el-option>
					</el-select>
				</el-form-item>
				<el-form-item>
					<el-input v-show="model==1" placeholder="时间，关键词查询" icon="search" v-model="para.Keyword" :on-icon-click="searchByTime"></el-input>
					<el-input v-show="model==2" placeholder="部门，关键词查询" icon="search" v-model="para.Keyword" :on-icon-click="searchByBranch"></el-input>
				</el-form-item>
			</el-form>
			<div class="pickTime" v-show="this.model==1">
				开始时间：
				<el-date-picker v-model="para.STime" type="date" placeholder="选择日期时间" clearable></el-date-picker>
				<span class="endTime">结束时间：</span>
				<el-date-picker v-model="para.ETime" type="date" placeholder="选择日期时间" clearable></el-date-picker>
			</div>
			<div class="pickTime" v-show="this.model==2">
				账号选择：
				<el-select v-model="para.BranchID" placeholder="请选择">
					<el-option v-for="item in branchs" :key="item.id" :label="item.branchName" :value="item.id"></el-option>
				</el-select>
			</div>
		</el-col>
	
		<!--列表-->
		<el-table :data="tableData" border style="width: 100%" v-loading="loading">
			<el-table-column prop="User" label="操作账号" align="center">
			</el-table-column>
			<el-table-column prop="LogTime" label="操作时间" align="center">
			</el-table-column>
			<el-table-column prop="IP" label="IP" align="center">
			</el-table-column>
			<el-table-column prop="LogInfo" label="操作信息" align="center">
			</el-table-column>
		</el-table>
	
		<!--工具条-->
		<el-col :span="24" class="toolbar">
			<el-col :span="12" :offset="5">
				<el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="this.para.page" :page-sizes="[10, 20, 50, 100]" :page-size="this.para.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
				</el-pagination>
			</el-col>
		</el-col>
	</div>
</template>

<script>
import { getLogs, getBranchInLog } from '../../api/api';
export default {
	data() {
		return {
			options: [{
				value: 1,
				label: '按时间查询'
			}, {
				value: 2,
				label: '按部门查询'
			}],
			branchs: [],
			tableData: [],
			model: 1,
			para: {
				token: '',
				Keyword: '',
				STime: '',
				ETime: '',
				BranchID: '',
				page: 1,
				pagesize: 10,
				user:''
			},
			loading: false,
			total: 0
		}
	},
	mounted() {
		this.fetchData();
		this.fetchBranch();
		this.para.user = JSON.parse(sessionStorage.getItem('user')).userName;
	},
	methods: {
		handleSizeChange(val) {
			this.para.pagesize = val;
			this.fetchData();
		},
		handleCurrentChange(val) {
			this.para.page = val;
			this.fetchData();
		},
		fetchData() {
			this.para.token = this.md5();
			if(this.model==1) this.para.BranchID='';
			this.loading = true;
			getLogs(this.para).then(res => {
				// console.log(res)
				this.tableData = res.data;
				this.total = res.total
				this.loading = false;
			})
		},
		formatTime(e) {
			let month = (e.getMonth() + 1).toString().padStart(2, '0');
			let day = e.getDate().toString().padStart(2, '0');
			return e.getFullYear() + '-' + month + '-' + day;
		},
		searchByTime() {
			if (!this.para.STime && !this.para.ETime) {

			} else if (this.para.STime && this.para.ETime) {
				this.para.STime = this.formatTime(this.para.STime);
				this.para.ETime = this.formatTime(this.para.ETime);
			} else {
				this.$message({
					message: '请选择一个时间段',
					type: 'error'
				});
				return;
			}
			this.fetchData();
			this.para.STime = '';
			this.para.ETime = '';
		},
		fetchBranch() {
			this.para.token = this.md5();
			getBranchInLog(this.para).then(res => {
				this.branchs = res;
			})
		},
		searchByBranch() {
			this.para.token = this.md5();
			this.fetchData();
		}

	}
}
</script>
<style lang="scss" scoped>
#log {
	.pickTime {
		padding-bottom: 10px;
		.endTime {
			margin-left: 20px;
		}
	}
}
</style>

 