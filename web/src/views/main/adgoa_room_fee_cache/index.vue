<template>
  <div class="adgoa_room_fee_cache-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="HotelId">
          <el-input v-model="queryParams.hotelId" clearable="" placeholder="请输入HotelId"/>
          
        </el-form-item>
        <el-form-item label="RoomId">
          <el-input v-model="queryParams.roomId" clearable="" placeholder="请输入RoomId"/>
          
        </el-form-item>
        <el-form-item label="MaxOccupancy">
          <el-input-number v-model="queryParams.maxOccupancy"  clearable="" placeholder="请输入MaxOccupancy"/>
          
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'adgoa_room_fee_cache:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
          </el-button-group>
          
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddadgoa_room_fee_cache" v-auth="'adgoa_room_fee_cache:add'"> 新增 </el-button>
          
        </el-form-item>
        
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table
				:data="tableData"
				style="width: 100%"
        max-height="1000"
				v-loading="loading"
				tooltip-effect="light"
				row-key="id"
				border="">
        <el-table-column type="index" label="序号" width="55" align="center"/>
         <el-table-column prop="hotelId" label="HotelId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="roomId" label="RoomId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="roomName" label="RoomName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="maxOccupancy" label="最大可入住" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price" label="1人入住pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_Two" label="2入住pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_Max" label="最大入住pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_1" label="21日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_2" label="22日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_TotalPayment_Consecutive_3" label="20日-23日连住三天pc总价" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_3" label="23日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_4" label="24日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_5" label="25日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_6" label="26日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_TotalPayment_Consecutive_7" label="20日-27日连住七天pc总价" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="pC_Price_N_Add_7" label="27日的pc价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price" label="20日app价格【1人入住】" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="apP_Price_Two" label="20日app价格【2人入住】" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="apP_Price_Max" label="20日app价格【最大入住】" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_1" label="21日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_2" label="22日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_3" label="23日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_4" label="24日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_5" label="25日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_6" label="26日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_Price_N_Add_7" label="27日app价格" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_TotalPayment_Consecutive_3" label="20日-23日连住app总价" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="apP_TotalPayment_Consecutive_7" label="20日-27日连住app总价" fixed="" show-overflow-tooltip="" />
       
       
         <el-table-column prop="status" label="Status" fixed="" show-overflow-tooltip="" />
       
      </el-table>
      <el-pagination
				v-model:currentPage="tableParams.page"
				v-model:page-size="tableParams.pageSize"
				:total="tableParams.total"
				:page-sizes="[100, 200, 500,]"
				small=""
				background=""
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
	/>
      <editDialog
			    ref="editDialogRef"
			    :title="editadgoa_room_fee_cacheTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="adgoa_room_fee_cache">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/adgoa_room_fee_cache/component/editDialog.vue'
  import { pageadgoa_room_fee_cache, deleteadgoa_room_fee_cache } from '/@/api/main/adgoa_room_fee_cache';


    const editDialogRef = ref();
    const loading = ref(false);
    const tableData = ref<any>
      ([]);
      const queryParams = ref<any>
        ({});
        const tableParams = ref({
        page: 1,
        pageSize: 100,
        total: 0,
        });
        const editadgoa_room_fee_cacheTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageadgoa_room_fee_cache(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddadgoa_room_fee_cache = () => {
        editadgoa_room_fee_cacheTitle.value = '添加agdoa数据';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditadgoa_room_fee_cache = (row: any) => {
        editadgoa_room_fee_cacheTitle.value = '编辑agdoa数据';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const deladgoa_room_fee_cache = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteadgoa_room_fee_cache(row);
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


