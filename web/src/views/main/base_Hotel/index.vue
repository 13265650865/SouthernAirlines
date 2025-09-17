<template>
  <div class="base_Hotel-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
     
     
      
        <el-form-item label="价格日期">
          <el-date-picker placeholder="请选择价格日期" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.pDateRange" />
          
        </el-form-item>
       
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'base_Hotel:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
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
				border="">
        <el-table-column type="index" label="序号" fixed="" show-overflow-tooltip=""/>
         <el-table-column prop="name" label="酒店名称" fixed="" show-overflow-tooltip="" >
           <template #default="scope">
              <el-link  type="primary" :href="scope.row.url" target="_blank">{{scope.row.name}}</el-link>
          </template> 
         </el-table-column>
         <el-table-column prop="score" label="评分" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="houseType" label="房间类型" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="bedType" label="床型" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="price" label="最低价格" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="rPrice" label="实际价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="hasWindow" label="窗户" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pDate" label="价格日期" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="scoreCount" label="评论数" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="">
           <template #default="scope">
                <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditBase_Hotel(scope.row)"> 查看房型 </el-button>
          </template> 
        </el-table-column> 
      </el-table>
    
      <editDialog
			    ref="editDialogRef"
			    :title="editBase_HotelTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="base_Hotel">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/base_Hotel/component/editDialog.vue'
  import { pageBase_Hotel, deleteBase_Hotel } from '/@/api/main/base_Hotel';


    const editDialogRef = ref();
    const loading = ref(false);
    const tableData = ref<any>([]);
    const queryParams = ref<any>({});
    const tableParams = ref({
        page: 1,
        pageSize: 20,
        total: 0,
    });
    const editBase_HotelTitle = ref("");
        

        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageBase_Hotel(Object.assign(queryParams.value, tableParams.value));
      
        tableData.value = res.data.result ?? [];       
        loading.value = false;
        };

        // 打开新增页面
        const openAddBase_Hotel = () => {
        editBase_HotelTitle.value = '添加酒店列表';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditBase_Hotel = (row: any) => {
        editBase_HotelTitle.value = '查看酒店信息';
        editDialogRef.value.openDialog(row);
        };
handleQuery();
</script>


