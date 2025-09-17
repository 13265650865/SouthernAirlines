<template>
    <div class="adgoa_room_fee_cache_r-container">
        <el-card shadow="hover" header="agdoa爬取数据统计图表">
            <el-form :model="queryParams" ref="queryForm" :inline="true">
                <el-row>
                    <el-col :span="24">
                        <el-radio-group v-model="queryParams.ctime" @input="rClick">
                            <el-radio-button label="0" border>pc,app价格差异统计</el-radio-button>
                            <el-radio-button label="1" border>不同入住人统计</el-radio-button>
                            <el-radio-button label="2" border>连住3天价格VS单晚差异统计</el-radio-button>
                            <el-radio-button label="3" border>连住7天价格VS单晚差异统计</el-radio-button>
                            <el-radio-button label="4" border>连住3天但单晚计划中断</el-radio-button>
                            <el-radio-button label="5" border>连住7天但单晚计划中断</el-radio-button>
                        </el-radio-group>
                    </el-col>
                    <el-col :span="24">
                        <el-tabs ref="tabsRef" v-model="activeName" @tab-click="handleClick">
                        <el-tab-pane label="20日数据" name="0">
                                <el-row :gutter="20">
                                <el-col :span="8">
                                    <div id="main"></div>
                                </el-col>
                                <el-col :span="16">
                                </el-col>
                                </el-row>

                        </el-tab-pane>
                        </el-tabs>
                    </el-col>
                </el-row>
            </el-form>
        </el-card>
    </div>
</template>
<script lang="ts" setup="" name="adgoa_room_fee_cache_r">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import * as echarts from 'echarts';
import {  GetOption } from '/@/api/main/adgoa_room_fee_cache';
const queryParams = ref({
  ctime: 0,
});
const activeName = ref('0')
const tabsValue = ref(0);
const rClick = (event: any) => {
    console.log(event.target.value);
}
const handleClick = async () => {
}
onMounted(() => {
  setTimeout(() => {
    initPieChart();
  }, 50);
})

const initPieChart = async () => {
  var chartDom = document.getElementById('main')!;

  var myChart = echarts.init(chartDom);
  myChart.resize({ width: 1000, height: 700 });
  var option: EChartsOption;
  var res = await GetOption();
  option = res.data.result;
  option && myChart.setOption(option, true); 
}
</script>