<template>
  <div class="planeComponent-container">
    <el-dialog v-model="isShowDialog" title="部件缺陷记录" fullscreen draggable="">
      <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
        <el-form :model="queryParams" ref="queryForm" :inline="true">
          <el-form-item label="部件件号" disabled>
            <el-input
              v-model="queryParams.componentPartNum"
              disabled
              placeholder="请输入部件件号"
            />
          </el-form-item>
          <el-form-item label="部件CMM" disabled>
            <el-input v-model="queryParams.cmm" disabled placeholder="请输入CMM" />
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
                v-auth="'componentDefectRecord:page'"
              >
                查询
              </el-button>
              <el-button
                icon="ele-Refresh"
                @click="
                  () =>
                    (queryParams = {
                      componentPartNum: queryParams.componentPartNum,
                      cmm: queryParams.cmm,
                      chineseDefectDescription: null,
                      replaceComponentPartNum: null,
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
                  <el-table-column
                    prop="quantity"
                    label="数量"
                    show-overflow-tooltip=""
                  />
                  <el-table-column prop="unit" label="单位" show-overflow-tooltip="" />
                </el-table>
              </div>
            </template>
          </el-table-column>
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
  </div>
</template>

<script lang="ts" setup="" name="componentDefectRecord">
import { ref, onMounted, reactive } from "vue";
import {
  pageComponentDefectRecord,
  deleteComponentDefectRecord,
  getUploadTemplateComponentDefectRecord,
} from "/@/api/main/componentDefectRecord";

const isShowDialog = ref(false);
const loading = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageComponentDefectRecord(
    Object.assign(queryParams.value, tableParams.value)
  );
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
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

const openDialog = async (row: any) => {
  queryParams.value = {
    componentPartNum: row.partNum,
    cmm: row.cmm,
    chineseDefectDescription: null,
    replaceComponentPartNum: null,
  };
  isShowDialog.value = true;
  await handleQuery();
};

onMounted(async () => {});

defineExpose({ openDialog });
</script>
