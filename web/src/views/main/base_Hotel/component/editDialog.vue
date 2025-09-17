<template>
	<div class="base_Hotel-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="900" draggable="">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="ruleForm.id" />
					</el-form-item>
					<el-col :xs="24" :sm="14" :md="14" :lg="14" :xl="14" class="mb20">
						<el-form-item label="酒店名称" prop="name">
							
							<el-tag  >{{ruleForm.name}}</el-tag>
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="14" :md="14" :lg="14" :xl="14" class="mb20">
						<el-form-item label="评分" prop="score">
							<el-rate
								v-model="ruleForm.score"
								disabled
								show-score
								text-color="#ff9900"
								score-template="{value}"
								colors="['#F7BA2A', '#F7BA2A', '#F7BA2A']"
								>
								</el-rate>
							
							</el-form-item>
						
					</el-col>
				
					<el-col :xs="24" :sm="14" :md="14" :lg="14" :xl="14" class="mb20">
						<el-form-item label="最低价格" prop="price">
							<el-tag  type="danger">{{ruleForm.price}}起</el-tag>
						</el-form-item>
					</el-col>
				
					<el-col :xs="24" :sm="14" :md="14" :lg="14" :xl="14" class="mb20">
						<el-form-item label="评论数" prop="scoreCount">
							<span>{{ruleForm.scoreCount}}</span>
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="14" :md="14" :lg="14" :xl="14" class="mb20">
						<el-form-item label="筛选日期" prop="pDate">
							 <el-date-picker placeholder="请选择价格日期" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.pDateRange" />
							<el-button type="primary" @click="submit" size="default" >查询</el-button>
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-table
							:data="getHotelDetail"
							border
							height="500"
							style="width: 100%"
							v-loading="loading">
							<el-table-column
								prop="pDate"
								label="日期"
								width="100"
								:formatter="formatterTime">
							</el-table-column>
							<el-table-column
								prop="houseType"
								label="房型"
								width="130">
							</el-table-column>
							<el-table-column
								prop="bedType"
								label="床型"
								width="120">
							</el-table-column>
							<el-table-column
								prop="rPrice"
								label="价格">
							</el-table-column>
							<el-table-column
								prop="hasWindow"
								label="窗户">
							</el-table-column>
							<el-table-column
								prop="smoking"
								label="禁烟政策">
							</el-table-column>
							<el-table-column
								prop="breakfast"
								label="早餐">
							</el-table-column>
							<el-table-column
								prop="cancelRule"
								label="取消规则"
								show-overflow-tooltip
								width="180">
							</el-table-column>
						</el-table>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
	import { ref,onMounted } from "vue";
	import { ElMessage } from "element-plus";
	import type { FormRules } from "element-plus";
	import { addBase_Hotel, updateBase_Hotel,detail_Hotel ,getHotDetail} from "/@/api/main/base_Hotel";
	//父级传递来的参数
	var props = defineProps({
	title: {
	type: String,
	default: "",
	},
	});
	
	const loading=ref(false);
	const queryParams = ref<any>({});
	const tableParams = ref({
        page: 1,
        pageSize: 20,
        total: 0,
        });
// 去除时间的时分秒
const formatterTime = (row, column) => {
    let data = row[column.property]
    return /\d{4}-\d{1,2}-\d{1,2}/g.exec(data)
}
	//父级传递来的函数，用于回调
	const getHotelDetail = ref([]);//酒店房型信息
	const emit = defineEmits(["reloadTable"]);
	const ruleFormRef = ref();

	const isShowDialog = ref(false);
	const ruleForm = ref<any>({});
		//自行添加其他规则
		const rules = ref<FormRules>({
});


// 打开弹窗
const openDialog = (row: any) => {
  ruleForm.value = JSON.parse(JSON.stringify(row));
  
   getDetail(ruleForm.value.hotId)
	
  isShowDialog.value = true;
};
const getDetail = async (e:any) =>{

getHotelDetail.value= (await detail_Hotel({id:e})).data.result ;
	
};
// 关闭弹窗
const closeDialog = () => {
  emit("reloadTable");
  isShowDialog.value = false;
  loading.value=false;
};

// 取消
const cancel = () => {
  isShowDialog.value = false;
  loading.value=false;
};

// 提交
const submit = async () => {
	queryParams.value.hotId=ruleForm.value.hotId;
	loading.value=true;
	var res = await getHotDetail(Object.assign(queryParams.value, tableParams.value));
	getHotelDetail.value= res.data.result ;
	loading.value=false;
};





// 页面加载时
onMounted(async () => {
	
});
//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




