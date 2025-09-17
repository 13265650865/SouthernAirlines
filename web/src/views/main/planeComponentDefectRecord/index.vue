<template>
  <div class="planeComponentDefectRecord-container">
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
        <el-form-item label="飞机注册号" prop="planeId">
          <el-select
            v-model="queryParams.planeId"
            filterable
            remote
            reserve-keyword
            no-data-text="无机队数据"
            placeholder="请输入注册号"
            :remote-method="queryPlane"
            :loading="planeLoading"
          >
            <el-option
              v-for="item in planeOptions"
              :key="item.id"
              :label="item.regisId"
              :value="item.id"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="部件CMM">
          <el-input v-model="queryParams.cmm" clearable="" placeholder="请输入CMM" />
        </el-form-item>
        <el-form-item label="部件件号">
          <el-input
            v-model="queryParams.componentPartNum"
            clearable=""
            placeholder="请输入部件件号"
          />
        </el-form-item>
        <el-form-item label="中文缺陷描述">
          <el-input
            v-model="queryParams.chineseDefectDescription"
            clearable=""
            placeholder="请输入中文缺陷描述"
          />
        </el-form-item>
        <el-form-item label="替换部件件号">
          <el-input
            v-model="queryParams.replaceComponentPartNum"
            clearable=""
            placeholder="请输入替换部件件号"
          />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button
              type="primary"
              icon="ele-Search"
              @click="handleQuery"
              v-auth="'planeComponentDefectRecord:page'"
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
            @click="openAddPlaneComponentDefectRecord"
            v-auth="'planeComponentDefectRecord:add'"
          >
            新增
          </el-button>
          <el-button
            type="primary"
            icon="ele-Download"
            @click="getPlaneComponentDefectRecordUploadTemplate"
            v-auth="'planeComponentDefectRecord:add'"
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
        <el-table-column type="expand" fixed>
          <template #default="props">
            <div m="4">
              <el-table :data="props.row.details" :border="childBorder">
                <el-table-column
                  prop="englishDefectDescription"
                  label="部件英文缺陷描述"
                  show-overflow-tooltip=""
                >
                  <template #default="scope">
                    {{ props.row.chineseDefectDescription }}
                  </template>
                </el-table-column>
                <el-table-column
                  prop="englishDefectDescription"
                  label="部件英文缺陷描述"
                  show-overflow-tooltip=""
                />
                <el-table-column
                  prop="replaceComponentPartNum"
                  label="替换部件件号"
                  show-overflow-tooltip=""
                />
                <el-table-column
                  prop="componenDescription"
                  label="部件描述"
                  show-overflow-tooltip=""
                />
                <el-table-column prop="quantity" label="数量" show-overflow-tooltip="" />
                <el-table-column prop="unit" label="单位" show-overflow-tooltip="" />
              </el-table>
            </div>
          </template>
        </el-table-column>
        <el-table-column
          prop="planeFleetCode"
          label="机队代号"
          fixed
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="planeRegisId"
          label="飞机注册号"
          fixed
          show-overflow-tooltip=""
        />
        <el-table-column
          prop="componentPartNum"
          label="部件件号"
          fixed
          show-overflow-tooltip=""
        />
        <el-table-column prop="cmm" label="部件CMM" fixed show-overflow-tooltip="" />
        <el-table-column
          prop="chineseDefectDescription"
          label="部件中文缺陷描述"
          show-overflow-tooltip=""
        />
        <el-table-column
          label="操作"
          width="140"
          align="center"
          fixed="right"
          show-overflow-tooltip=""
          v-if="
            auth('planeComponentDefectRecord:edit') ||
            auth('planeComponentDefectRecord:delete')
          "
        >
          <template #default="scope">
            <el-button
              icon="ele-Edit"
              size="small"
              text=""
              type="primary"
              @click="openEditPlaneComponentDefectRecord(scope.row)"
              v-auth="'planeComponentDefectRecord:edit'"
            >
              编辑
            </el-button>
            <el-button
              icon="ele-Delete"
              size="small"
              text=""
              type="primary"
              @click="delPlaneComponentDefectRecord(scope.row)"
              v-auth="'planeComponentDefectRecord:delete'"
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
        :title="editPlaneComponentDefectRecordTitle"
        @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="planeComponentDefectRecord">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from "/@/utils/authFunction";
import { listPlaneFleet } from "/@/api/main/planeFleet";
import { listPlane } from "/@/api/main/base_Airplane";
import { getToken } from "/@/utils/axios-utils";

const authHeaders = ref<any>({
  Authorization: `Bearer ${getToken()}`,
});
const uploadUrl = ref(
  `${import.meta.env.VITE_API_URL}/api/planeComponentDefectRecord/upload`
);
const fullscreenLoading = ref(false);
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

const getPlaneComponentDefectRecordUploadTemplate = () => {
  getUploadTemplatePlaneComponentDefectRecord().then((res) => {
    const { data, headers } = res;
    const blob = new Blob([data], { type: headers["content-type"] });
    let dom = document.createElement("a");
    let url = window.URL.createObjectURL(blob);
    dom.href = url;
    dom.download = "PlaneComponentDefectRecord.xlsx";
    dom.style.display = "none";
    document.body.appendChild(dom);
    dom.click();
    dom.parentNode.removeChild(dom);
    window.URL.revokeObjectURL(url);
  });
};

const planeFleetLoading = ref(false);
const planeFleetOptions = ref<any>([]);

const queryPlaneFleet = async (key) => {
  planeFleetLoading.value = true;
  var res = await listPlaneFleet({ code: key });
  planeFleetOptions.value = res.data.result ?? [];
  planeFleetLoading.value = false;
};

const planeLoading = ref(false);
const planeOptions = ref<any>([]);

const queryPlane = async (key) => {
  planeLoading.value = true;
  var res = await listPlane({
    planeFleetId: queryParams.value.planeFleetId,
    regisId: key,
  });
  planeOptions.value = res.data.result ?? [];
  planeLoading.value = false;
};

import editDialog from "/@/views/main/planeComponentDefectRecord/component/editDialog.vue";
import {
  pagePlaneComponentDefectRecord,
  deletePlaneComponentDefectRecord,
  getUploadTemplatePlaneComponentDefectRecord,
} from "/@/api/main/planeComponentDefectRecord";

const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editPlaneComponentDefectRecordTitle = ref("");

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pagePlaneComponentDefectRecord(
    Object.assign(queryParams.value, tableParams.value)
  );
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddPlaneComponentDefectRecord = () => {
  editPlaneComponentDefectRecordTitle.value = "添加飞机部件缺陷记录";
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditPlaneComponentDefectRecord = (row: any) => {
  editPlaneComponentDefectRecordTitle.value = "编辑飞机部件缺陷记录";
  editDialogRef.value.openDialog(row);
};

// 删除
const delPlaneComponentDefectRecord = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deletePlaneComponentDefectRecord(row);
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
