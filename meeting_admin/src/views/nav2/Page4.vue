<template>
  <div>
    <!--上部工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
      <el-form :inline="true">
        <el-form-item>
          <el-button type="primary" icon="plus" @click="editRow(-1, tableData)">新增</el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="danger" icon="delete" @click="deleteRows()" :disabled="this.sels.length===0">删除</el-button>
        </el-form-item>
        <el-form-item style="float:right">
          <el-input placeholder="按标题、内容查询" icon="search" v-model="para.searchKey"></el-input>
        </el-form-item>
      </el-form>
    </el-col>
  
    <!--列表-->
    <el-table ref="multipleTable" v-loading="loading" :data="tableData" border tooltip-effect="dark" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="selection" width="55" align="center">
      </el-table-column>
      <el-table-column label="模版标题" width="150" align="center">
        <template scope="scope">{{ scope.row.Title }}</template>
      </el-table-column>
      <el-table-column prop="Content" label="内容" align="center">
      </el-table-column>
      <el-table-column prop="CreateTime" label="创建时间" width="200" align="center">
      </el-table-column>
      <el-table-column label="操作" width="150" align="center">
        <template scope="scope">
          <el-button type="text" size="small" @click.native.prevent="editRow(scope.$index, tableData)">编辑</el-button>
          <el-button size="small" type="text" @click.native.prevent="deleteRow(scope.$index, tableData)">删除</i>
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  
    <!--编辑模版-->
    <el-dialog :title='this.para.index>=0?"编辑模版":"新建模版"' :visible.sync="dialogFormVisible">
      <el-form :model="para">
        <el-form-item label="模版标题" :label-width="formLabelWidth">
          <el-input v-model="para.value.Title" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="短信内容" :label-width="formLabelWidth">
          <el-input type="textarea" v-model="para.value.Content" auto-complete="off"></el-input>
          <el-col>当前共{{length}}字（不含签名）</el-col>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="confirm">保 存</el-button>
      </div>
    </el-dialog>
  
  </div>
</template>

<script>
import { getTextTemplate, updateTemplate, createTemplate, deleteTemplate, batchDelete } from '../../api/api';
export default {
  data() {
    return {
      dialogFormVisible: false,//编辑页面是否显示
      sels: [],//列表选中项
      tableData: [],
      formLabelWidth: '120px',
      loading: false,
      para: {
        token: '',
        searchKey: '',
        index: -1,
        user:'',
        value: {
          TemplateID: '',
          Title: '',
          Content: '',
        },
        templateIDs: ''
      }
    }
  },
  watch: {
    'para.searchKey': 'fetchData',
    deep: true,
  },
  computed: {
    length: function () {
      return !this.para.value.Content?0:this.para.value.Content.length;
    }
  },
  mounted() {
    this.para.user = JSON.parse(sessionStorage.getItem('user')).userName;
    this.fetchData();
  },
  methods: {
    deleteRow(index, rows) {
      this.para.value.TemplateID = this.tableData[index].TemplateID;
      this.$confirm('确认删除选中记录吗？', '提示', {
        type: 'warning'
      }).then(() => {
        this.para.token = this.md5();
        deleteTemplate(this.para).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
          this.fetchData();
        })
      })
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
    //批量删除
    deleteRows() {
      this.$confirm('确认删除选中的所有记录吗？', '提示', {
        type: 'warning'
      }).then(() => {
        //NProgress.start();
        this.para.templateIDs = this.sels.map(item => item.TemplateID).toString();
        this.para.token = this.md5();
        batchDelete(this.para).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
          this.fetchData();
        });
      }).catch(() => {

      });
    },
    editRow(index, rows) {
      this.para.index = -1;
      this.para.value.Title = '';
      this.para.value.Content = '';
      this.para.value.TemplateID = '';
      if (index < 0) {
        this.dialogFormVisible = true;
        return;
      }
      this.para.value.Title = this.tableData[index].Title;
      this.para.value.Content = this.tableData[index].Content;
      this.para.value.TemplateID = this.tableData[index].TemplateID;
      this.para.index = index;
      this.dialogFormVisible = true;
    },
    confirm() {
      if (!this.para.value.Title && !this.para.value.Content) {
        this.dialogFormVisible = false;
        return;
      }
      if (this.para.index >= 0) {
        this.tableData[this.para.index].Title = this.para.value.Title;
        this.tableData[this.para.index].Content = this.para.value.Content;
        this.update();
        this.dialogFormVisible = false;
      } else {
        this.create();
      }
    },
    //编辑更新
    update() {
      this.para.token = this.md5();
      updateTemplate(this.para).then(res => {
        this.$message({
          message: '操作成功',
          type: 'success'
        });
      })
    },
    //创建新增
    create() {
      this.para.token = this.md5();
      createTemplate(this.para).then(res => {
        this.dialogFormVisible = false;
        this.fetchData();
        this.$message({
          message: '操作成功',
          type: 'success'
        });
      })
    },
    //获取列表
    fetchData() {
      this.para.token = this.md5();
      this.loading = true;
      getTextTemplate(this.para).then(res => {
        this.tableData = res;
        this.loading = false;
      })
    }
  }
}
</script>