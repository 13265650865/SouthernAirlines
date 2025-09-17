<template>
  <div class="component-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="部件类别">
          <el-select
            v-model="queryParams.componentCategoryId"
            filterable
            remote
            reserve-keyword
            no-data-text="无部件类别"
            placeholder="请输入部件类别"
            :remote-method="queryComponentCategory"
            :loading="componentCategoryLoading"
          >
            <el-option
              v-for="item in componentCategoryOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="件号">
          <el-input v-model="queryParams.partNum" clearable="" placeholder="请输入件号" />
        </el-form-item>
        <el-form-item label="厂家">
          <el-input
            v-model="queryParams.factoryName"
            clearable=""
            placeholder="请输入厂家"
          />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button
              type="primary"
              icon="ele-Search"
              @click="handleQuery"
              v-auth="'component:page'"
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
            @click="openAddComponent"
            v-auth="'component:add'"
          >
            新增
          </el-button>
          <el-button
            type="primary"
            icon="ele-Download"
            @click="getComponentUploadTemplate"
            v-auth="'component:add'"
          >
            导入模板
          </el-button>
          <el-upload
            accept=".xlsx"
            :action="uploadUrl"
            style="height: 24px; margin-left: 12px"
            :headers="authHeaders"
            v-loading.fullscreen.lock="fullscreenLoading"
            :on-error="closeLoading"
            :on-progress="showLoading"
            :on-success="uploadSuccess"
            v-auth="'component:add'"
          >
            <el-button type="primary" icon="ele-UploadFilled">导入</el-button>
          </el-upload>
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
        <el-table-column
          prop="componentCategoryName"
          label="部件类别"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="description"
          label="描述"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column prop="partNum" label="件号" fixed="" show-overflow-tooltip="" />
        <el-table-column
          prop="factoryName"
          label="厂家"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column prop="quantity" label="数量" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="unit" label="单位" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="remark" label="备注" fixed="" show-overflow-tooltip="" />
        <el-table-column
          label="操作"
          width="140"
          align="center"
          fixed="right"
          show-overflow-tooltip=""
          v-if="auth('component:edit') || auth('component:delete')"
        >
          <template #default="scope">
            <el-button
              icon="ele-Edit"
              size="small"
              text=""
              type="primary"
              @click="openEditComponent(scope.row)"
              v-auth="'component:edit'"
            >
              编辑
            </el-button>
            <el-button
              icon="ele-Delete"
              size="small"
              text=""
              type="primary"
              @click="delComponent(scope.row)"
              v-auth="'component:delete'"
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
        :title="editComponentTitle"
        @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="component">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from "/@/utils/authFunction";
import { getToken } from "/@/utils/axios-utils";

import editDialog from "/@/views/main/component/component/editDialog.vue";
import {
  pageComponent,
  deleteComponent,
  getUploadTemplateComponent,
} from "/@/api/main/component";
import { listComponentCategory } from "/@/api/main/componentCategory";
const authHeaders = ref<any>({
  Authorization: `Bearer ${getToken()}`,
});

const fullscreenLoading = ref(false);
const uploadUrl = ref(`${import.meta.env.VITE_API_URL}/api/component/upload`);
const showLoading = () => {
  fullscreenLoading.value = true;
};
const closeLoading = () => {
  fullscreenLoading.value = false;
};
const uploadSuccess = async (res) => {
  if (res.code == 200) {
    ElMessage({
      message: "导入成功",
      type: "info",
    });
    await handleQuery();
  } else {
    ElMessage({
      message: res.message,
      type: "error",
    });
  }
  fullscreenLoading.value = false;
};

const getComponentUploadTemplate = () => {
  getUploadTemplateComponent().then((res) => {
    const { data, headers } = res;
    const blob = new Blob([data], { type: headers["content-type"] });
    let dom = document.createElement("a");
    let url = window.URL.createObjectURL(blob);
    dom.href = url;
    dom.download = "Component.xlsx";
    dom.style.display = "none";
    document.body.appendChild(dom);
    dom.click();
    dom.parentNode.removeChild(dom);
    window.URL.revokeObjectURL(url);
  });
};

const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editComponentTitle = ref("");
const componentCategoryLoading = ref(false);
const componentCategoryOptions = ref<any>([]);

const queryComponentCategory = async (key) => {
  componentCategoryLoading.value = true;
  var res = await listComponentCategory({ name: key });
  componentCategoryOptions.value = res.data.result ?? [];
  componentCategoryLoading.value = false;
};

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageComponent(
    Object.assign(queryParams.value, tableParams.value, { type: 1 })
  );
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddComponent = () => {
  editComponentTitle.value = "添加部件";
  editDialogRef.value.openDialog({ type: 1 });
};

// 打开编辑页面
const openEditComponent = (row: any) => {
  editComponentTitle.value = "编辑部件";
  editDialogRef.value.openDialog(row);
};

// 删除
const delComponent = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteComponent(row);
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
