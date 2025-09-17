<template>
  <div class="base_Statistics-container">
    <el-card shadow="hover" header="访问流量">
      <el-divider content-position="left">今日访问流量</el-divider>
      <el-row :gutter="20">
        <el-col :span="6">
          <div>
            <el-statistic group-separator="," :value="dlist.tPV" title="浏览量(PV)"></el-statistic>
          </div>
        </el-col>
        <el-col :span="6">
          <div>
            <el-statistic title="访客数" :value="dlist.tF">
              <template state.slot="formatter">
                456/2
              </template>
            </el-statistic>
          </div>
        </el-col>
        <el-col :span="4">
          <div>
            <el-statistic group-separator="," :precision="2" decimal-separator="." value="0" title="增长数[PV]">
              <template state.slot="prefix">
                <i class="el-icon-s-flag" style="color: red"></i>
              </template>
              <template state.slot="suffix">
                <i class="el-icon-s-flag" style="color: blue"></i>
              </template>
            </el-statistic>
          </div>
        </el-col>
        <el-col :span="4">
          <div>
            <el-statistic :value="dlist.tAccessCount" title="访问次数">
              <template state.slot="suffix">
              </template>
            </el-statistic>
          </div>
        </el-col>
        <el-col :span="4">
          <div>
            <el-statistic :value="dlist.tIPCount" title="IP数">
              <template state.slot="suffix">
              </template>
            </el-statistic>
          </div>
        </el-col>
      </el-row>
    </el-card>
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="访问时间">
          <el-date-picker placeholder="请选择访问时间" value-format="YYYY/MM/DD" type="daterange"
            v-model="queryParams.accessDateRange" />

        </el-form-item>
        <el-form-item label="来源">
          <el-input v-model="queryParams.source" clearable="" placeholder="请输入来源" />

        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'base_Statistics:page'"> 查询
            </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>

          </el-button-group>

        </el-form-item>
        <el-form-item>
          <!-- <el-button type="primary" icon="ele-Plus" @click="openAddBase_Statistics" v-auth="'base_Statistics:add'"> 新增 </el-button> -->

        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column type="expand" fixed="">
          <template #default="prop">
            <el-form label-position="left" inline class="demo-table-expand">
              <el-form-item label="访问类型：">
                <span>{{ prop.row.isOld === true ? "老访客" : "新访客" }}</span>
              </el-form-item>
              <el-form-item label="上次访问时间：">
                <span>{{ }}</span>
              </el-form-item>
              <el-form-item label="本次来路：">
                <span>{{ prop.row.source }}</span>
              </el-form-item>
              <el-form-item label="入口页面：">
              
                <a href="{{ prop.row.url }}">{{ prop.row.url  }}</a>
              </el-form-item>
              <el-form-item label="最后停留在：">
                <a href="{{ prop.row.lastAccessUrl }}">{{ prop.row.lastAccessUrl }}</a>
              </el-form-item>

              <el-card shadow="hover" header="访问路径">
                <el-timeline :reverse="reverse">
                  <el-timeline-item v-for="(activity, index) in activities" :key="index" :timestamp="activity.timestamp">
                    <a href="{{ activity.content }}">{{ activity.content }}</a>
                  </el-timeline-item>
                </el-timeline>
              </el-card>

            </el-form>
          </template>
        </el-table-column>
        <el-table-column type="index" label="序号" fixed="" />
        <el-table-column prop="url" label="访问url" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="ipAddress" label="IP地址" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="ipArea" label="IP区域" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="accessDate" label="访问时间" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="source" label="来源" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="accessCount" label="访问页数" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="lastAccessUrl" label="最后停留地址" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="isOld" label="是否老访客" show-overflow-tooltip="">
          <template #default="scope">
            <el-tag v-if="scope.row.isOld"> 是 </el-tag>
            <el-tag type="danger" v-else=""> 否 </el-tag>

          </template>

        </el-table-column>
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('base_Statistics:edit') || auth('base_Statistics:delete')">
          <template #default="scope">
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delBase_Statistics(scope.row)"
              v-auth="'base_Statistics:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background="" @size-change="handleSizeChange"
        @current-change="handleCurrentChange" layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editBase_StatisticsTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="base_Statistics">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/base_Statistics/component/editDialog.vue'
import { pageBase_Statistics, deleteBase_Statistics, getDate_Statistics } from '/@/api/main/base_Statistics';


const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editBase_StatisticsTitle = ref("");
const reverse = ref(true);
const activities = ref<any>([{
  content: 'http://localhost:5065/login/index',
  timestamp: '2023-10-07 11:55:09'
}, {
  content: 'http://localhost:5065/swagger',
  timestamp: '2023-10-07 10:23:00'
}, {
  content: 'http://localhost:5065/index.html',
  timestamp: '2023-10-07 10:20:00'
}]);
const dlist = ref<any>({});
const ylist = ref<any>({});
// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageBase_Statistics(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
  var response = await getDate_Statistics();
  dlist.value = response.data.result[0] ?? [];
  //ylist.value = response.data.result[0] ?? [];
};

// 打开新增页面
const openAddBase_Statistics = () => {
  editBase_StatisticsTitle.value = '添加数据统计信息';
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditBase_Statistics = (row: any) => {
  editBase_StatisticsTitle.value = '编辑数据统计信息';
  editDialogRef.value.openDialog(row);
};

// 删除
const delBase_Statistics = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteBase_Statistics(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => { });
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
<style lang="scss">
.like {
  cursor: pointer;
  font-size: 25px;
  display: inline-block;
}

.demo-table-expand {
  font-size: 0;
}

.demo-table-expand label {
  width: 90px;
  color: #99a9bf;
}

.demo-table-expand .el-form-item {
  margin-right: 0;
  margin-bottom: 0;
  width: 100%;
}
</style>


