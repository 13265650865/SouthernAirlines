<template>
  <div class="planeComponent-container">
    <el-dialog v-model="isShowDialog" title="飞机部件" :width="1000" draggable="">
      <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
        <el-form :model="queryParams" ref="queryForm" :inline="true">
          <el-form-item label="飞机注册号"> {{ queryParams.regisId }} </el-form-item>
          <el-form-item label="件号">
            <el-input
              v-model="queryParams.partNum"
              clearable=""
              placeholder="请输入件号"
            />
          </el-form-item>
          <el-form-item label="CMM">
            <el-input v-model="queryParams.cmm" clearable="" placeholder="请输入CMM" />
          </el-form-item>
          <el-form-item>
            <el-button-group>
              <el-button type="primary" icon="ele-Search" @click="handleQuery">
                查询
              </el-button>
              <el-button
                icon="ele-Refresh"
                @click="
                  () =>
                    (queryParams = {
                      regisId: queryParams.regisId,
                      planeId: queryParams.planeId,
                    })
                "
              >
                重置
              </el-button>
            </el-button-group>
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
          <el-table-column type="index" width="55" fixed="" />
          <el-table-column
            prop="componentDescription"
            label="描述"
            show-overflow-tooltip=""
          />
          <el-table-column prop="partNum" label="件号" show-overflow-tooltip="" />
          <el-table-column prop="cmm" label="CMM" show-overflow-tooltip="" />
          <el-table-column prop="eff" label="EFF" show-overflow-tooltip="" />
          <el-table-column prop="factoryName" label="厂家" show-overflow-tooltip="" />
          <el-table-column
            label="操作"
            align="center"
            fixed="right"
            show-overflow-tooltip=""
          >
            <template #default="scope">
              <el-button
                icon="ele-View"
                size="small"
                text=""
                type="primary"
                @click="openQueryComponentDefectRecord(scope.row)"
                v-auth="'base_Airplane:edit'"
              >
                缺陷故障
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
      </el-card>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="() => (isShowDialog = false)" size="default"
            >取 消</el-button
          >
        </span>
      </template>
    </el-dialog>
    <queryComponentDefectRecord ref="queryComponentDefectRecordRef" />
  </div>
</template>

<script lang="ts" setup="" name="queryPlaneComponentDialog">
import { ref, onMounted, reactive } from "vue";
import { pagePlaneComponent } from "/@/api/main/base_Airplane";
import queryComponentDefectRecord from "/@/views/main/base_Airplane/component/queryComponentDefectRecord.vue";

const queryComponentDefectRecordRef = ref();
const loading = ref(false);
const isShowDialog = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});

const handleQuery = async () => {
  loading.value = true;
  var res = await pagePlaneComponent(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

const handleSizeChange = (val: number) => {
  tableParams.value.pageSize = val;
  handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
  tableParams.value.page = val;
  handleQuery();
};

const openDialog = async (row: any) => {
  queryParams.value = {
    planeId: row.id,
    regisId: row.regisId,
    partNum: null,
    cmm: null,
  };
  isShowDialog.value = true;
  await handleQuery();
};

const openQueryComponentDefectRecord = (row: any) => {
  queryComponentDefectRecordRef.value.openDialog(row);
};

onMounted(async () => {});

defineExpose({ openDialog });
</script>
