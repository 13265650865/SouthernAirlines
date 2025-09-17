<template>
  <div class="notice-bar-container layout-pd">
    <el-card shadow="hover" header="统计图表">
      <el-row :gutter="20">
        <el-col :span="6">
          <div id="main"></div>
        </el-col>
        <el-col :span="18"><div id="zhu"></div></el-col>


      </el-row>
      <el-row :gutter="20">
        <el-col :span="24">
          <div id="fx"></div>
        </el-col>
      </el-row>
    </el-card>


  </div>
</template>

<script setup lang="ts" name="chartSIndex">
import { reactive, onMounted, nextTick } from 'vue';
import { getDate_Statistics } from '/@/api/main/base_Statistics';
import * as echarts from 'echarts';
type EChartsOption = echarts.EChartsOption;


 // 查询操作
 const handleQuery = async () => {      
        var res = await getDate_Statistics();
     
  };
onMounted(() => {

  setTimeout(() => {
    initPieChart();
    initZhuChart();
  }, 700);
  // nextTick(() => {})

})
//饼图
const initPieChart = () => {
  var chartDom = document.getElementById('main')!;
  var myChart = echarts.init(chartDom);
  myChart.resize({ width: 300, height: 300 });
  var option: EChartsOption;

  option = {
    tooltip: {
      trigger: 'item'
    },
    legend: {
      top: '5%',
      left: 'center'
    },
    series: [
      {
        name: '来源分析',
        type: 'pie',
        radius: ['40%', '70%'],
        avoidLabelOverlap: false,
        label: {
          show: false,
          position: 'center'
        },
        emphasis: {
          label: {
            show: true,
            fontSize: 40,
            fontWeight: 'bold'
          }
        },
        labelLine: {
          show: false
        },
        data: [
          { value: 1, name: 'FireFox' },
          { value: 1, name: 'Edge' },
          { value: 3, name: 'Google' }
        ]
      }
    ]
  };

  option && myChart.setOption(option);

}
//折线图
const initZhuChart = () => {
  var chartDom = document.getElementById('zhu')!;
  var myChart = echarts.init(chartDom);
  myChart.resize({ width: 1000, height: 300 });
  var option: EChartsOption;
  option = {
  title: {
    text: '浏览量PV统计'
  },
  tooltip: {
    trigger: 'axis'
  },
  legend: {
    data: ['搜索引擎', '直接访问', '外部链接', '自定义来源']
  },
  grid: {
    left: '3%',
    right: '4%',
    bottom: '3%',
    containLabel: true
  },
  toolbox: {
    feature: {
      saveAsImage: {}
    }
  },
  xAxis: {
    type: 'category',
    boundaryGap: false,
    data: ['0:00-02::59   浏览量(PV)', '3:00-05:99    浏览量(PV)', '6:00-08:59    浏览量(PV)', '9:00-11:59    浏览量(PV)', '1200-23:59    浏览量(PV)']
  },
  yAxis: {
    type: 'value'
  },
  series: [
    {
      name: '搜索引擎',
      type: 'line',
      stack: 'Total',
      data: [0, 0, 1, 1]
    },
    {
      name: '直接访问',
      type: 'line',
      stack: 'Total',
      data: [1, 2, 2, 1]
    },
    {
      name: '外部链接',
      type: 'line',
      stack: 'Total',
      data: [0, 0, 0, 0, 0]
    },
    {
      name: '自定义来源',
      type: 'line',
      stack: 'Total',
      data: [0, 0, 0, 0]
    }
  ]
};
  option && myChart.setOption(option);
}
handleQuery();
</script>
<style lang="scss">
.like {
  cursor: pointer;
  font-size: 25px;
  display: inline-block;
}
</style>