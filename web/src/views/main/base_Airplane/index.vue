<template>
  <div class="base_Airplane-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="所属机队">
          <el-select
            v-model="queryParams.planeFleetId"
            filterable
            remote
            reserve-keyword
            no-data-text="无机队数据"
            placeholder="请输入机队号"
            :remote-method="queryPlaneFleet"
            :loading="planeFleetLoading"
          >
            <el-option
              v-for="item in planeFleetOptions"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="注册号">
          <el-input
            v-model="queryParams.regisId"
            clearable=""
            placeholder="请输入注册号"
          />
        </el-form-item>
        <el-form-item label="机型名">
          <el-input
            v-model="queryParams.planeModelName"
            clearable=""
            placeholder="请输入机型名"
          />
        </el-form-item>
        <el-form-item label="机型号">
          <el-input
            v-model="queryParams.planeModel"
            clearable=""
            placeholder="请输入机型号"
          />
        </el-form-item>
        <el-form-item label="MSN">
          <el-input v-model="queryParams.msn" clearable="" placeholder="请输入MSN" />
        </el-form-item>
        <el-form-item label="VARTAB">
          <el-input
            v-model="queryParams.vartab"
            clearable=""
            placeholder="请输入VARTAB"
          />
        </el-form-item>
        <el-form-item label="AMM_EFF">
          <el-input
            v-model="queryParams.ammEFF"
            clearable=""
            placeholder="请输入AMM_EFF"
          />
        </el-form-item>
        <el-form-item label="IPC_EFF">
          <el-input
            v-model="queryParams.ipcEFF"
            clearable=""
            placeholder="请输入IPC_EFF"
          />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button
              type="primary"
              icon="ele-Search"
              @click="handleQuery"
              v-auth="'base_Airplane:page'"
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
            @click="openAddBase_Airplane"
            v-auth="'base_Airplane:add'"
          >
            新增
          </el-button>
          <el-button
            type="primary"
            icon="ele-UploadFilled"
            @click="openUploadPlaneComponent"
            v-auth="'base_Airplane:add'"
          >
            导入
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
        <el-table-column
          type="index"
          label="序号"
          width="55"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="regisId"
          label="注册号"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="planeModelName"
          label="机型名"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="planeModelNo"
          label="机型号"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column prop="msn" label="MSN" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="vartab" label="VARTAB" fixed="" show-overflow-tooltip="" />
        <el-table-column
          prop="ammEFF"
          label="AMM_EFF"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="ipcEFF"
          label="IPC_EFF"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="planeFleetCode"
          label="所属机队"
          fixed=""
          show-overflow-tooltip=""
        />
        <el-table-column
          label="操作"
          width="240"
          align="center"
          fixed="right"
          show-overflow-tooltip=""
          v-if="auth('base_Airplane:edit') || auth('base_Airplane:delete')"
        >
          <template #default="scope">
            <el-button
              icon="ele-View"
              size="small"
              text=""
              type="primary"
              @click="openQueryPlaneComponent(scope.row)"
              v-auth="'base_Airplane:edit'"
            >
              部件详情
            </el-button>
            <el-button
              icon="ele-Edit"
              size="small"
              text=""
              type="primary"
              @click="openEditBase_Airplane(scope.row)"
              v-auth="'base_Airplane:edit'"
            >
              编辑
            </el-button>
            <el-button
              icon="ele-Delete"
              size="small"
              text=""
              type="primary"
              @click="delBase_Airplane(scope.row)"
              v-auth="'base_Airplane:delete'"
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
      <queryPlaneComponentDialog ref="queryPlaneComponentRef" />
      <editDialog
        ref="editDialogRef"
        :title="editBase_AirplaneTitle"
        @reloadTable="handleQuery"
      />
      <uploadDialog ref="uploadDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="base_Airplane">
import { ref } from "vue";
import type { FormRules } from "element-plus";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from "/@/utils/authFunction";
import { getToken } from "/@/utils/axios-utils";

import queryPlaneComponentDialog from "/@/views/main/base_Airplane/component/queryPlaneComponentDialog.vue";
import editDialog from "/@/views/main/base_Airplane/component/editDialog.vue";
import uploadDialog from "/@/views/main/base_Airplane/component/uploadDialog.vue";
import {
  pageBase_Airplane,
  deleteBase_Airplane,
  getUploadTemplatePlane,
} from "/@/api/main/base_Airplane";
import { listPlaneFleet } from "/@/api/main/planeFleet";

const uploadUrl = ref(`${import.meta.env.VITE_API_URL}/api/base_Airplane/upload`);
const queryPlaneComponentRef = ref();
const editDialogRef = ref();
const uploadDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const planeFleetLoading = ref(false);
const planeFleetOptions = ref<any>([]);
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editBase_AirplaneTitle = ref("");

const openUploadPlaneComponent = () => {
  uploadDialogRef.value.openDialog({});
};

const queryPlaneFleet = async (key) => {
  planeFleetLoading.value = true;
  var res = await listPlaneFleet({ code: key });
  planeFleetOptions.value = res.data.result ?? [];
  planeFleetLoading.value = false;
};

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageBase_Airplane(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddBase_Airplane = () => {
  editBase_AirplaneTitle.value = "添加飞机设置";
  editDialogRef.value.openDialog({});
};

const openQueryPlaneComponent = (row: any) => {
  queryPlaneComponentRef.value.openDialog(row);
};

// 打开编辑页面
const openEditBase_Airplane = (row: any) => {
  editBase_AirplaneTitle.value = "编辑飞机设置";
  editDialogRef.value.openDialog(row);
};

const getPlaneUploadTemplate = () => {
  getUploadTemplatePlane().then((res) => {
    const { data, headers } = res;
    const blob = new Blob([data], { type: headers["content-type"] });
    let dom = document.createElement("a");
    let url = window.URL.createObjectURL(blob);
    dom.href = url;
    dom.download = "PlaneComponent.xlsx";
    dom.style.display = "none";
    document.body.appendChild(dom);
    dom.click();
    dom.parentNode.removeChild(dom);
    window.URL.revokeObjectURL(url);
  });
};

// 删除
const delBase_Airplane = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteBase_Airplane(row);
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
