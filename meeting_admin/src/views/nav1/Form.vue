<template>

	<el-form ref="form" :model="form" label-width="80px" @submit.prevent="onSubmit" style="margin:20px;width:80%;min-width:400px;">
	
			<el-steps :space="400" :active="active" finish-status="success">
			<el-step title="1 基本信息" ></el-step>
			<el-step title="2 会议议程(选填)" ></el-step>
			<el-step title="3 会务信息(选填)" ></el-step>
			</el-steps>

	  <el-collapse v-model="activeNames" @change="handleChange" >
		<el-collapse-item title="1 基本信息" name='1'>
			<div>
					<el-form-item label="会议名称" style="width:60%;">
						<el-input v-model="form.meetingInfo.MeetingName" placeholder="请输入会议名称"></el-input>
					</el-form-item>
					<el-form-item label="会议地点" style="width:60%;">
							<el-input v-model="form.meetingInfo.MeetingAddr" placeholder="请输入会议地点"></el-input>
					</el-form-item>
					<el-form-item label="开始时间">
						<el-col :span="11">
							<el-date-picker format="yyyy-MM-dd HH:mm" type="datetime" placeholder="选择开始日期" v-model="form.meetingInfo.StartTime" style="width: 100%;"></el-date-picker>
						</el-col>
					</el-form-item>
					<el-form-item label="结束时间">
							<el-col :span="11">
							<el-date-picker format="yyyy-MM-dd HH:mm" type="datetime" placeholder="选择结束日期" v-model="form.meetingInfo.EndTime" style="width: 100%;"></el-date-picker>
							</el-col>
					</el-form-item>
				
					<el-form-item label="会议性质">
					 <el-radio-group v-model="form.meetingInfo.MeetingNature">
							<el-radio :label="0">私密</el-radio>
							<el-radio :label="1" >开放</el-radio>
						</el-radio-group> 
					</el-form-item>
					<el-form-item label="参会人员" v-model="form.meetingInfo.tag">
						<el-radio-group >
							<el-tag>上传</el-tag>
							<el-tag type="gray">标签二</el-tag>
							<el-tag type="primary">标签三</el-tag>
						</el-radio-group>
					</el-form-item>
					<el-form-item label="会议摘要">
							<el-input v-model="form.meetingInfo.MeetingSummary" placeholder="请输入会议摘要"></el-input>
					</el-form-item>
					<el-form-item label="会议说明">
						<el-input type="textarea" v-model="form.meetingInfo.MeetingContent" placeholder="请输入会议说明"></el-input>
					</el-form-item>
					<el-form-item label="上传文件">
						
						<el-upload class="upload-demo" ref="upload" :data="data" :action="url" :on-success="handleSuccess" :on-preview="handlePreview" :on-remove="handleRemove" :file-list="fileList" :auto-upload="false">
						<el-button slot="trigger" size="small" type="primary">选取文件</el-button>
						<!--<el-button style="margin-left: 10px;" size="small" type="success" @click="submitUpload">上传到服务器</el-button>-->
						<div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
						</el-upload>
					</el-form-item>
					
					<el-form-item label="通知方式">
						 <el-checkbox-group v-model="form.meetingInfo.SendMethod">
							<el-checkbox label=0>即时通信(App)</el-checkbox>
							<el-checkbox label=1>短信</el-checkbox>
						 </el-checkbox-group>
					</el-form-item>
					<el-form-item label="短信内容">
						<el-input type="textarea" v-model="form.meetingInfo.MessageContent" placeholder="如不填写短信内容，默认发送内容：各位领导、同事：请于xxxx时间于xxxx会议室准时参加xxx会议，详细会议情况请在办公助手软件上查阅。"></el-input>
					</el-form-item>
					<el-form-item>
						<el-form-item label="短信模板">
							<template>
								<el-select v-model="form.meetingInfo.TemplateID" placeholder="请选择模板">
										<el-option label="请选择模板" value="0"></el-option>
									<el-option label="模板一" value="1"></el-option>	
									<el-option label="模板二" value="2"></el-option>	
								</el-select>
							</template>

					</el-form-item>
					</el-form-item>
						<el-form-item label="通知时间">
						<el-checkbox-group v-model="form.meetingInfo.SendTimeFlag">
							<el-checkbox label=0>会议发布后</el-checkbox>
						 </el-checkbox-group>
						
					</el-form-item>  
			</div>
		</el-collapse-item>
		<el-collapse-item title="2 会议议程（选填）" name='2'>
			<div>
			 <el-form-item v-for="(domain,index) in form.meetingInfo.MeetingAgenda" :prop="domain.UUID">
			    
				<el-form-item label="议程主题" style="width:60%;">
					<el-input v-model="domain.Title" placeholder="请输入议程主题"></el-input>
				</el-form-item>
				<el-form-item label="开始时间">
					<el-col :span="11">
						<el-date-picker type="datetime" format="yyyy-MM-dd HH:mm" placeholder="选择开始日期" v-model="domain.STime" style="width: 100%;"></el-date-picker>
					</el-col>
				</el-form-item>
				<el-form-item label="结束时间">
					<el-col :span="11">
					<el-date-picker type="datetime" format="yyyy-MM-dd HH:mm" placeholder="选择结束日期" v-model="domain.ETime" style="width: 100%;"></el-date-picker>
					</el-col>
				</el-form-item>
				<el-form-item label="主持人" style="width:60%;">
						<el-input v-model="domain.Name" placeholder="主持人"></el-input>
				</el-form-item>
				<el-form-item label="内容摘要">
						<el-input v-model="domain.Summary" placeholder="请输入内容摘要"></el-input>
				</el-form-item>
				<el-form-item label="详细内容">
					<el-input type="textarea" v-model="domain.Content" placeholder="请输入详细内容"></el-input>
				</el-form-item>
	            	<el-button @click.prevent="removeDomain(domain)">删除</el-button>
			</el-form-item>
				   <el-button @click="addDomain">新增会议议程</el-button>
			</div>
		</el-collapse-item>
		 <el-collapse-item title="3 会务信息（选填）" name='3'>
			<div>

				<el-form-item label="作息安排" style="width:80%;">
				 <el-button @click="addDomain2">新增作息安排时间</el-button>
				</el-form-item>
			    <el-form-item v-for="(domain2, index) in form.meetingInfo.MeetingBusiness[0].Schedule" :prop="domain2.UUID">
				<el-form-item label="事项" style="width:60%;">
					<el-input v-model="domain2.Item" placeholder="事务内容"></el-input>
				</el-form-item>
				<el-form-item label="开始时间">
					<el-col :span="11">
						<el-date-picker type="datetime" placeholder="选择开始日期" v-model="domain2.STime" style="width: 100%;"></el-date-picker>
					</el-col>
				</el-form-item>
				<el-form-item label="结束时间">
					<el-col :span="11">
					<el-date-picker type="datetime" placeholder="选择结束日期" v-model="domain2.ETime" style="width: 100%;"></el-date-picker>
			    	</el-col>
				</el-form-item>
				<el-button @click.prevent="removeDomain2(domain2)">删除</el-button>
				 </el-form-item>

				<el-form-item label="会议物品" style="width:80%;">
						<el-input type="textarea" v-model="form.meetingInfo.MeetingBusiness[0].MeetingItems" placeholder="请输入需要物品"></el-input>
				</el-form-item>

				<el-form-item label="接送安排" style="width:80%;">
				 <el-button @click="addDomain3">新增接送安排时间</el-button>
				</el-form-item>
				<el-form-item v-for="(domain3, index) in form.meetingInfo.MeetingBusiness[0].Pickup" :prop="domain3.UUID">
				<el-form-item label="事项" style="width:60%;">
					<el-input v-model="domain3.Item" placeholder="事务内容"></el-input>
				</el-form-item>
				<el-form-item label="开始时间">
					<el-col :span="11">
						<el-date-picker type="datetime" placeholder="选择开始日期" v-model="domain3.STime" style="width: 100%;"></el-date-picker>
					</el-col>
				</el-form-item>
				<el-form-item label="结束时间">
					<el-col :span="11">
					<el-date-picker type="datetime" placeholder="选择结束日期" v-model="domain3.ETime" style="width: 100%;"></el-date-picker>
			    	</el-col>
				</el-form-item>
				<el-button @click.prevent="removeDomain3(domain3)">删除</el-button>
				 </el-form-item>

				<el-form-item label="务组信息" style="width:80%;">
				</el-form-item>
			    <el-form-item label="会务地址" style="width:80%;">
					<el-input v-model="form.meetingInfo.MeetingBusiness[0].RAddress" placeholder="请输入会务地址"></el-input>
			    </el-form-item>
				<el-form-item label="会务电话" style="width:80%;"> 
					<el-input v-model="form.meetingInfo.MeetingBusiness[0].RTell" placeholder="请输入会务电话"></el-input>
			    </el-form-item>
				<el-form-item label="接待时间" style="width:80%;"> 
					<el-col :span="11">
					<el-date-picker type="datetime" format="yyyy-MM-dd HH:mm" placeholder="请输入接待时间" v-model="form.meetingInfo.MeetingBusiness[0].RTime" style="width: 100%;"></el-date-picker>
			    </el-col>
			    </el-form-item>

				<el-form-item label="注意事项" style="width:80%;">
					<el-input type="textarea" v-model="form.meetingInfo.MeetingBusiness[0].Matters" placeholder="请输入会议说明"></el-input>
				</el-form-item>
				
			</div>
		</el-collapse-item> 
      </el-collapse>
		
		<el-form-item>
		  <!--  <el-button style="margin-top: 12px;" @click="next">下一步</el-button> -->
			<el-button type="success" @click.native="AddSubmit()">提交</el-button>
			<el-button @click.native.prevent type="primary">取消</el-button>
		</el-form-item>
	</el-form>
	
</template>

<script>
    import { addMeeting, uploadFile } from '../../api/api';
	import util from '../../common/js/util'
	export default {
		data() {
			return {
				 active: 0,
				activeNames: '1',
				fileList: [],
				url:uploadFile,
				data:{token:this.md5()},
				form: {
					token:this.md5(),
					meetingInfo: 
						{
						MeetingName: '123',
						MeetingAddr: '123',
						StartTime: '2017-07-19 17:12',
						EndTime: '2017-08-19 17:12',
						MeetingNature: 0,
						tag: '',
						MeetingSummary:'订单123',
						MeetingContent: '订单订单',
						uploap: '',

						SendMethod:['0'],
						MessageContent:'qweqweq',
						TemplateID:'1',
						SendTimeFlag:['0'],

						CreateUser:'',
						CreateTime:'',
						UpdateUser:'',
						UpdateTime:'',

						MeetingAgenda:[
							{
								Title:'测试主题',
								STime:'2017-07-19 19:12',
								ETime:'2017-08-19 13:12',
								Name:'测试人',
								Summary:'测试',
								Content:'测试'
							}
						],
						MeetingMember:[],
						MeetingBusiness:[
							{
								Schedule:[{
									value:'',
									Item:'',
									STime:'',
									ETime:''
								}],
								Pickup:[{
									value:'',
									Item:'',
									STime:'',
									ETime:''
								}],
								MeetingItems:'',
								RAddress:'',
								RTell:'',
								RTime:'',
								Matters:''
							}]
						},
					Files:[{
						UUID:'',
						FileUUID:'',
						FileName:'',
						FileExtension:'',
						Url:'',
						UploadUser:'string',
						UploadTime:'string',
						FileLength:'0',
						DownloadTimes:'0',
						Flag:'0',
						Download:[
							{
							DownloadUser: 'string',
							DownloadTime: 'string'
							}
						]
					}],
				}
			}
		},
			
		methods:{

			handleSuccess(data) {
				this.form.Files.push(data);
				console.log(this.form.Files);
			},
			submitUpload() {
				console.log(this.$refs.upload);
      		}, 
			handleRemove(file, fileList) {
       			 console.log(file, fileList);
     		},
      		handlePreview(file) {
       			 console.log(file);
      		},
			handleChange(){
				if(this.activeNames ='2')
				{
					this.active =2;
				}
			},
			handleSelect(key, keyPath) {},

			AddSubmit:function(){
				this.$refs.upload.submit();
				this.$refs.form.validate((valid) => {
					if(valid){
                    this.$confirm("确认提交",'提示',{}).then(() => {
					 
                     this.form.meetingInfo.MeetingAgenda[0].STime =util.formatDate.format(new Date(this.form.meetingInfo.MeetingAgenda[0].STime), 'yyyy-MM-dd hh:mm');
                     this.form.meetingInfo.MeetingAgenda[0].ETime =util.formatDate.format(new Date(this.form.meetingInfo.MeetingAgenda[0].ETime), 'yyyy-MM-dd hh:mm');
                     this.form.meetingInfo.MeetingBusiness[0].Schedule[0].STime =util.formatDate.format(new Date(this.form.meetingInfo.MeetingBusiness[0].Schedule[0].STime), 'yyyy-MM-dd hh:mm');
                     this.form.meetingInfo.MeetingBusiness[0].Schedule[0].ETime =util.formatDate.format(new Date(this.form.meetingInfo.MeetingBusiness[0].Schedule[0].ETime), 'yyyy-MM-dd hh:mm');
                     this.form.meetingInfo.MeetingBusiness[0].RTime =util.formatDate.format(new Date(this.form.meetingInfo.MeetingBusiness[0].RTime), 'yyyy-MM-dd hh:mm');
                     this.form.meetingInfo.MeetingAgenda[0].StartTime = (!this.form.meetingInfo.MeetingAgenda[0].StartTime || this.form.meetingInfo.MeetingAgenda[0].StartTime == '') ? '' : util.formatDate.format(new Date(this.form.meetingInfo.MeetingAgenda[0].StartTime), 'yyyy-MM-dd hh:mm');
                     this.form.meetingInfo.MeetingAgenda[0].EndTime = (!this.form.meetingInfo.MeetingAgenda[0].EndTime || this.form.meetingInfo.MeetingAgenda[0].EndTime == '') ? '' : util.formatDate.format(new Date(this.form.meetingInfo.MeetingAgenda[0].EndTime), 'yyyy-MM-dd hh:mm');
                     
                    let para =Object.assign({},this.form);
                    addMeeting(para).then(() => {
					 console.log(para);
                        this.$message({
                            message:'提交成功！',
                            type:'success'
                        });
                        this.$refs['form'].resetFields();
                        //this.$router.push({path:'/form'})
                    })
                    }).catch(() => {
                    });
				 }
				})
			},
			removeDomain(index) {
				this.form.meetingInfo.MeetingAgenda.splice(index, 1)
			},
			addDomain() {
        		this.form.meetingInfo.MeetingAgenda.push({   //初始值
        	});
			},
			addDomain2() {
        		this.form.meetingInfo.MeetingBusiness[0].Schedule.push({
        	});
			},
			removeDomain2(index) {
				this.form.meetingInfo.MeetingBusiness[0].Schedule.splice(index, 1)
				
			},
			addDomain3() {
        		this.form.meetingInfo.MeetingBusiness[0].Pickup.push({   //初始值
          		/*value:'',        
				Item:'',
				STime:'',
				ETime:'',
          		key: Date.now()*/
        	});
			},
			removeDomain3(index) {
			/*	var index = this.form.meetingInfo.MeetingBusiness[0].Pickup.indexOf(item)
				if (index !== -1) {  不用在进行判断找Index   */
				this.form.meetingInfo.MeetingBusiness[0].Pickup.splice(index, 1)  
				
			},
			onSubmit() {
				console.log('submit!');
			}
		},
		mounted(){

		}
	}

</script>