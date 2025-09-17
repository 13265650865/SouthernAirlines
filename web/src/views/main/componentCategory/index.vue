<template>
  <div class="componentCategory-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="名称">
          <el-input v-model="queryParams.name" clearable="" placeholder="请输入名称" />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button
              type="primary"
              icon="ele-Search"
              @click="handleQuery"
              v-auth="'componentCategory:page'"
            >
              查询
            </el-button>
            <el-button icon="ele-Refresh" @click="() => (queryParams = {})">
              重置
            </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            icon="ele-Plus"
            @click="openAddComponentCategory"
            v-auth="'componentCategory:add'"
          >
            新增
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table
        :data="tableData"
        style="width: 100%"
        v-loading="loading"
        tooltip-effect="light"
        row-key="id"
        border=""
      >
        <el-table-column type="index" label="序号" width="55" align="center" />
        <el-table-column prop="name" label="名称" fixed="" show-overflow-tooltip="" />
        <el-table-column
          label="操作"
          width="140"
          align="center"
          fixed="right"
          show-overflow-tooltip=""
          v-if="auth('componentCategory:edit') || auth('componentCategory:delete')"
        >
          <template #default="scope">
            <el-button
              icon="ele-Edit"
              size="small"
              text=""
              type="primary"
              @click="openEditComponentCategory(scope.row)"
              v-auth="'componentCategory:edit'"
            >
              编辑
            </el-button>
            <el-button
              icon="ele-Delete"
              size="small"
              text=""
              type="primary"
              @click="delComponentCategory(scope.row)"
              v-auth="'componentCategory:delete'"
            >
              删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        v-model:currentPage="tableParams.page"
        v-model:page-size="tableParams.pageSize"
        :total="tableParams.total"
        :page-sizes="[10, 20, 50, 100]"
        small=""
        background=""
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper"
      />
      <editDialog
        ref="editDialogRef"
        :title="editComponentCategoryTitle"
        @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="componentCategory">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from "/@/utils/authFunction";
//import { formatDate } from '/@/utils/formatTime';

import editDialog from "/@/views/main/componentCategory/component/editDialog.vue";
import {
  pageComponentCategory,
  deleteComponentCategory,
} from "/@/api/main/componentCategory";

const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editComponentCategoryTitle = ref("");

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageComponentCategory(
    Object.assign(queryParams.value, tableParams.value)
  );
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddComponentCategory = () => {
  editComponentCategoryTitle.value = "添加部件类别";
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditComponentCategory = (row: any) => {
  editComponentCategoryTitle.value = "编辑部件类别";
  editDialogRef.value.openDialog(row);
};

// 删除
const delComponentCategory = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteComponentCategory(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => {});
};

// 改变页面容量
const handleSizeChange = (val: number) => {
  tableParams.value.pageSize = val;
  handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
  tableParams.value.page = val;
  handleQuery();
};

handleQuery();
</script>
