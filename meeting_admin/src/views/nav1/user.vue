<template>

	<el-form>

		<!--工具条 -->
			<el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
			<el-form :inline="true">
				<el-form-item>
					<el-input v-model="DisName" placeholder="名称"></el-input>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" @click="SelectUser">查询</el-button>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" @click="showMyData">添加</el-button>
				</el-form-item>
			</el-form>
		</el-col>
	
		<el-table :data="users" :border="true" tooltip-effect="dark" max-height="200" style="width: 100%" @selection-change="handleSelectionChange">
			<el-table-column type="selection" width="55">
			</el-table-column>
			 <!--<el-table-column label="单位" width="120">
			<template scope="scope">{{ scope.row.date }}</template> 
			</el-table-column>-->
			<el-table-column prop="displayName" label="用户名" width="120">
			</el-table-column>
			<el-table-column prop="company" label="单位" width="120">
			</el-table-column>
			<el-table-column prop="mobile" label="手机号" show-overflow-tooltip>
			</el-table-column>
		</el-table>
		<!--<div style="margin-top: 20px">
			<el-button @click="toggleSelection([tableData3[1], tableData3[2]])">切换第二、第三行的选中状态</el-button>
			<el-button @click="toggleSelection()">取消选择</el-button>
		</div>  -->
	<el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
			<el-form :inline="true" >
			<el-tree
				:data="data2"
				show-checkbox
				node-key="id"
				:props="defaultProps">
				</el-tree>
			</el-form>
		</el-col>
	</el-form>	
</template>

<script>
 
 import { modelbd, userList,getBranchs,getallUsers } from '../../api/api';
 import util from '../../common/js/util';
	export default {
		data() {	
			return {
				open: false,
                isFolder: true,
				
				filters:[],
				DisName:'',
				users:[],
				branchsUsers:[],
				multipleSelection: [],
				data2: [{id:0,label:'部门',children:[]}],
				defaultProps: {
					
					children: 'children',
					label: 'label'
				},
			}
		},
	   
		mounted() {
			  var para ={token:this.md5()};
				 userList(para).then((data) => {
					 this.users =data;
				 });
			
			modelbd(para).then((data) => {
					this.branchsUsers = data;
					this.filterInfo(this.data2[0])});
		},
		methods: {
			filterInfo(child){           //filter 过滤器过滤
				this.branchsUsers.filter(v => v.parentID == child.id 
				).forEach(v => {
					child.children.push({
						id:v.id,
						label:v.name,
						ifBranch: !!(v.type == "Branch"),
						children:[]
					})
				})
				child.children.filter(e => e.ifBranch).map(this.filterInfo)
			},
			showMyData () {
					if(this.DisName != "")
					{
						this.users=this.searchByRegExp(this.DisName,this.users);
					}else{
						let para ={token:this.md5()}
						userList(para).then((data) => {this.users =data;});
					}
			},
			SelectUser(){
			},

			handleSelectionChange(val) {
				this.multipleSelection = val;
			},
			onSubmit() {
				console.log('submit!');
			},
			searchByRegExp(keyword, list) {      //将关键字和数组中的数据进行匹配
				if (!(list instanceof Array)) {
					return;
				}
				var arr = [];
				var reg = new RegExp(keyword);
				for (let i = 0; i < list.length; i++) {
					if (list[i].displayName.match(reg)) {
					arr.push(list[i]);
					}
				}
				return arr;
		    },
		
		},
	}

</script>