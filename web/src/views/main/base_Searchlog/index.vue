<template>
  <div class="base_Searchlog-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="爬取时间">
          <el-date-picker placeholder="请选择爬取时间" value-format="YYYY/MM/DD HH/mm/ss" type="daterange"
            v-model="queryParams.accessDateRange" />
          
        </el-form-item>
        <el-form-item>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" > 查询
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
				border="">
         <el-table-column prop="checkin" label="checkin" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="checkout" label="checkout" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="adult" label="adult" show-overflow-tooltip="">
        </el-table-column>
        <el-table-column prop="children" label="children" show-overflow-tooltip="">
        </el-table-column>
         <el-table-column prop="spendtime" label="耗时" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="category" label="分类  0 预抓  1实时" show-overflow-tooltip="">
        </el-table-column>
        <el-table-column prop="hasprice" label="是否有价" show-overflow-tooltip="">
        </el-table-column>
         <el-table-column prop="createTime" label="createTime" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('base_Searchlog:edit') || auth('base_Searchlog:delete')">
        
        </el-table-column>
      </el-table>
      <el-pagination
				v-model:currentPage="tableParams.page"
				v-model:page-size="tableParams.pageSize"
				:total="tableParams.total"
				:page-sizes="[50, 100]"
				small=""
				background=""
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
	/>
      <editDialog
			    ref="editDialogRef"
			    :title="editBase_SearchlogTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="base_Searchlog">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/base_Searchlog/component/editDialog.vue'
  import { pageBase_Searchlog, deleteBase_Searchlog } from '/@/api/main/base_Searchlog';


    const editDialogRef = ref();
    const loading = ref(false);
    const tableData = ref<any>
      ([]);
      const queryParams = ref<any>
        ({});
        const tableParams = ref({
        page: 1,
        pageSize: 50,
        total: 0,
        });
        const editBase_SearchlogTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageBase_Searchlog(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddBase_Searchlog = () => {
        editBase_SearchlogTitle.value = '添加爬虫数据统计';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditBase_Searchlog = (row: any) => {
        editBase_SearchlogTitle.value = '编辑爬虫数据统计';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delBase_Searchlog = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteBase_Searchlog(row);
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


