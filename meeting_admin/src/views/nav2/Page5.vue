<template>
  <div>
    <!--上部工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
      <el-form :inline="true">
        <el-form-item>
          <el-button type="primary" icon="plus" @click="editRow(-1, groupData)">添加分组</el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="danger" icon="delete" @click="deleteRows()" :disabled="this.sels.length===0">删除</el-button>
        </el-form-item>
      </el-form>
    </el-col>
  
    <!--列表-->
    <el-table ref="multipleTable" v-loading="loading" :data="groupData" border style="width: 100%" @selection-change="selsChange">
      <el-table-column type="selection" width="55" align="center"></el-table-column>
      <el-table-column label="分组名称"  align="center">
        <template scope="scope">{{ scope.row.GroupName }}</template>
      </el-table-column>
      <el-table-column prop="Comment" label="备注"  align="center"></el-table-column>
      <el-table-column label="操作" width="200" align="center">
        <template scope="scope">
          <el-button type="text" size="small" @click.native.prevent="editRow(scope.$index, groupData)">编辑</el-button>
          <el-button size="small" type="text" @click.native.prevent="deleteRow(scope.$index, groupData)">人员管理</i>
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  
    <!--编辑模版-->
    <el-dialog :title='this.para.index>=0?"编辑分组":"新建分组"' :visible.sync="dialogFormVisible">
      <el-form :model="para">
        <el-form-item label="名称" :label-width="formLabelWidth">
          <el-input v-model="para.value.GroupName" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="备注" :label-width="formLabelWidth">
          <el-input type="textarea" v-model="para.value.Comment" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="confirm">保 存</el-button>
      </div>
    </el-dialog>
    <!--<el-tree :data="dataF" show-checkbox default-expand-all node-key="id" ref="tree" highlight-current :props="defaultProps">-->
    </el-tree>
  </div>
</template>

	<script>
import { getUsers, getBranch, getGroup, updateGroup, createGroup,batchDeleteGroup } from '../../api/api';
export default {
  data() {
    return {
      dialogFormVisible: false,//编辑页面是否显示
      formLabelWidth: '120px',
      groupData: [],
      sels: [],
      loading: false,
      para: {
        token: '',
        index: -1,
        user:'',
        value: {
          GroupID: '',
          GroupName: '',
          Comment: '',
        },
        GroupIDs: ''
      },
      // dataF: [],
      // defaultProps: {
      //   children: 'children',
      //   label: 'name'
      // }
    }
  },
  mounted() {
    this.para.user = JSON.parse(sessionStorage.getItem('user')).userName;
    this.fetchData();
  },
  methods: {
    fetchData() {
      this.para.token = this.md5();
      this.loading = true;
      getGroup(this.para).then(res => {
        this.groupData = res;
        this.loading = false;
      })
    },
    editRow(index, rows) {
      this.para.index = -1;
      this.para.value.GroupID = '';
      this.para.value.GroupName = '';
      this.para.value.Comment = '';
      if (index < 0) {
        this.dialogFormVisible = true;
        return;
      }
      this.para.value.GroupID = this.groupData[index].GroupID;
      this.para.value.GroupName = this.groupData[index].GroupName;
      this.para.value.Comment = this.groupData[index].Comment;
      this.para.index = index;
      this.dialogFormVisible = true;
    },
    confirm() {
      if (!this.para.value.GroupName && !this.para.value.Comment) {
        this.dialogFormVisible = false;
        return;
      }
      if (this.para.index >= 0) {
        this.groupData[this.para.index].GroupName = this.para.value.GroupName;
        this.groupData[this.para.index].Comment = this.para.value.Comment;
        this.update();
        this.dialogFormVisible = false;
      } else {
        this.create();
      }
    },
    //编辑更新
    update() {
      this.para.token = this.md5();
      updateGroup(this.para).then(res => {
        this.$message({
          message: '操作成功',
          type: 'success'
        });
      })
    },
    //创建新增
    create() {
      this.para.token = this.md5();
      createGroup(this.para).then(res => {
        this.dialogFormVisible = false;
        this.fetchData();
        this.$message({
          message: '操作成功',
          type: 'success'
        });
      })
    },
    selsChange(sels) {
      this.sels = sels;
    },
    //批量删除
    deleteRows() {
      this.$confirm('确认删除选中的所有记录吗？', '提示', {
        type: 'warning'
      }).then(() => {
        //NProgress.start();
        this.para.GroupIDs = this.sels.map(item => item.GroupID).toString();
        this.para.token = this.md5();
        batchDeleteGroup(this.para).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
          this.fetchData();
        });
      }).catch(() => {

      });
    }
    // fliterData(arr) {
    //   for (let i = 0; i < arr.length; i++) {
    //     if (arr[i].type === "Branch") {
    //       arr[i].children=[];
    //       this.dataF.push(arr[i]);
    //     }
    //     if (arr[i].type === "Leaf") {
    //       for(let j=0;j<this.dataF.length;j++){
    //         if(this.dataF[j].id==arr[i].parentID){
    //           this.dataF[j].children.push(arr[i]);
    //         }
    //       }
    //     }
    //   }
    //   console.log(this.dataF)
    // }
  }
}
</script>
