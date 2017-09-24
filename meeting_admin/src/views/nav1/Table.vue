<template>
	<section>
		<!--工具条-->
		<el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
			<el-form :inline="true" :model="filters">
				<el-form-item>
					<el-input v-model="filters.meetingName" placeholder="名称"></el-input>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" v-on:click="getUsers">查询</el-button>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" @click="handleAdd">创建会议</el-button>
				</el-form-item>
			</el-form>
		</el-col>
		<el-col>
		<el-menu :default-active="activeIndex2" class="el-menu-demo" mode="horizontal" @select="handleSelect">
				<el-menu-item index="0" >所有会议</el-menu-item>
				<el-menu-item index="1" >进行会议</el-menu-item>
				<el-menu-item index="2" >待开会议</el-menu-item>
				<el-menu-item index="3" >已开会议</el-menu-item>	
				<el-menu-item index="4" >关闭会议</el-menu-item>	
		</el-menu>
		</el-col>

		<!--列表-->
		<el-table :data="users" highlight-current-row v-loading="listLoading" :border="true" style="width: 100%;">
			<el-table-column type="index" width="60">
			</el-table-column>
			<el-table-column prop="MeetingName" label="会议名称" width="150">
			</el-table-column>
		    <el-table-column prop="MeetingAddr" label="会议地点" width="180" >
			</el-table-column>
		    <el-table-column prop="StartTime" label="开始时间" width="180">
			</el-table-column>
			<el-table-column prop="MeetingMember.length" label="与会人数/报名人数" width="100" >
			</el-table-column>
			<el-table-column prop="MeetingStatus" :formatter="formatStatus" label="会议状态" min-width="100" >
			</el-table-column>
			<el-table-column label="操作" width="160" >
				
				<template scope="scope">
					<router-link :to="{ name: '编辑会议',  params:scope.row }" class="el-icon-document">
					</router-link> 
					<a @click="AddMeetingtubiao()" class="el-icon-edit"></a>
					<a @click="SMS()" class="el-icon-message"></a>
					<a @click="uploadMeeting()" class="el-icon-upload2"></a>
					<a @click="handleDel(scope.$index, scope.row)" class="el-icon-delete"></a>
				</template>
			</el-table-column>
		</el-table>

		<!--工具条-->
		<el-col :span="24" class="toolbar">
			<el-pagination layout="sizes, prev, pager, next" @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="page" :page-sizes="[5,10,15,20]" :page-size="pagesize" :total="total" style="float:right;">
			</el-pagination>
		</el-col>

	</section>
</template>

<script>
	import util from '../../common/js/util'
	import { removeMeeting, MeetingList, MeetingListPage, MeetingStatusList } from '../../api/api';
	
	export default {
		data() {
			return {
				activeIndex2: '0',
				filters: {
					meetingName: ''
				},
				users: [],
				total: 0,
				page: 1,
				pagesize:10,
				listLoading: false,
				userEdit:[]
			}
		},
		methods: {
            handleSelect(key, keyPath) {
				
				if(key == '0')
				{
					this.getUsers();
				}else if(key =='1')
				{
					let para = {
					token:this.md5(),
					name: this.filters.meetingName.trim(),
					meetingStatus:3,
					publishStatus:''
				};
				this.listLoading = true;
				MeetingList(para).then((data) => {
					this.users = data;
					this.listLoading = false;
				});
				}else if(key =='2')
				{
					let para = {
					token:this.md5(),
					name: this.filters.meetingName.trim(),
					meetingStatus:4,
					publishStatus:''
				};
				this.listLoading = true;
				MeetingList(para).then((data) => {
					this.users = data;
					this.listLoading = false;
				});
				}else if(key =='3')
				{
					let para = {
					token:this.md5(),
					name: this.filters.meetingName.trim(),
					meetingStatus:2,
					publishStatus:''
				};
				this.listLoading = true;
				MeetingList(para).then((data) => {
					this.users = data;
					this.listLoading = false;
				});
				}else if(key =='4')
				{
					let para = {
					token:this.md5(),
					name: this.filters.meetingName.trim(),
					meetingStatus:1,
					publishStatus:''
				};
				this.listLoading = true;
				MeetingList(para).then((data) => {
					this.users = data;
					this.listLoading = false;
				});
				}
 				
            },
			CopeMeeting(index,row){
				//this.$store.commit("INCREMENT",row);
				sessionStorage.setItem('editmeetingInfo', JSON.stringify(row));
				this.$router.push({path:'/Meeting'});
			},
		    formatStatus:function(row,column){
				 return row.MeetingStatus == 0 ? '编辑会议' : row.MeetingStatus == 1 ? '关闭会议' : row.MeetingStatus == 2 ? '待开会议' : row.MeetingStatus == 3 ? '进行会议' :row.MeetingStatus == 4 ? '结束会议' : '未知' ;
			},
			handleSizeChange(val){
				this.page = 1;
				this.pagesize = val;
				this.getUsers();
			},
			handleCurrentChange(val) {
				this.page = val;
				this.getUsers();
			},
			//获取用户列表
			getUsers() {
				let para = {
					token:this.md5(),
					page: this.page,
					pagesize:this.pagesize,
					name: this.filters.meetingName.trim(),
					meetingStatus:'',
					publishStatus:''
				};
				this.listLoading = true;
				
				MeetingListPage(para).then((data) => {
					this.total = data.total;
					this.users = data.meetings;
					this.listLoading = false;
				});
			},
			//删除
			handleDel: function (index, row) {
				this.$confirm('确认删除该记录吗?', '提示', {
					type: 'warning'
				}).then(() => {
					this.listLoading = true;
					//NProgress.start();
					let para = { uuid: row.UUID ,token:this.md5()};
					removeMeeting(para).then((data) => {
						this.listLoading = false;
						this.$message({
							message: '删除成功',
							type: 'success'
						});
						this.getUsers();
					});
				}).catch(() => {

				});
			},
			//新增界面
			handleAdd: function () {
				this.$router.push({ path: '/form' })
			}
		},
		mounted() {
			this.getUsers();
			
		}
	}

</script>

<style scoped>

</style>