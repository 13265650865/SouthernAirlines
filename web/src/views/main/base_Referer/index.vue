<template>
  <div class="base_Referer-container">
    <el-card shadow="hover" header="统计图表">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-row>
          <el-col :span="12">
            <el-radio-group v-model="queryParams.ctime" @input="rClick">
              <el-radio-button label="0" border>全部</el-radio-button>
              <el-radio-button label="1" border>10分钟内</el-radio-button>
              <el-radio-button label="2" border>当天</el-radio-button>
            </el-radio-group>

          </el-col>
          <el-col :span="24">
            <el-tabs ref="tabsRef" v-model="activeName" @tab-click="handleClick">
              <el-tab-pane label="来源" name="0">

                <el-row>
                  <el-col :span="5">
                    <el-form-item label="访问量：">
                      <span style="color: red; font-size: 24px;">{{ formData.pv }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="访客数：">
                      <span style="color: red; font-size: 24px;">{{ formData.ipcount }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"> </el-divider>
                </el-row>
                <el-row :gutter="20">
                  <el-col :span="8">
                    <div id="main0"></div>
                  </el-col>
                  <el-col :span="16">
                  </el-col>
                </el-row>

              </el-tab-pane>
              <el-tab-pane label="系统" name="1">
                <el-row>
                  <el-col :span="5">
                    <el-form-item label="访问量：">
                      <span style="color: red; font-size: 20px;">{{ formData.pv }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="访客数：">
                      <span style="color: red; font-size: 20px;">{{ formData.ipcount }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"> </el-divider>
                </el-row>
                <el-row :gutter="20" v-loading="loading">
                  <el-col :span="8">
                    <div id="main1"></div>
                  </el-col>
                  <el-col :span="16">
                  </el-col>
                </el-row>
              </el-tab-pane>
              <el-tab-pane label="IP地区" name="2">
                <el-row>
                  <el-col :span="5">
                    <el-form-item label="访问量：">
                      <span style="color: red; font-size: 20px;">{{ formData.pv }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="访客数：">
                      <span style="color: red; font-size: 20px;">{{ formData.ipcount }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"> </el-divider>
                </el-row>
                <el-row :gutter="20" v-loading="loading">
                  <el-col :span="8">
                    <div id="main2"></div>
                  </el-col>
                  <el-col :span="16">
                  </el-col>
                </el-row>
              </el-tab-pane>
              <el-tab-pane label="酒店" name="3">
                <el-row>
                  <el-col :span="5">
                    <el-form-item label="访问量：">
                      <span style="color: red; font-size: 20px;">{{ formData.pv }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="访客数：">
                      <span style="color: red; font-size: 20px;">{{ formData.ipcount }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"> </el-divider>
                </el-row>
                <el-row :gutter="20" v-loading="loading">
                  <el-col :span="8">
                    <div id="main3"></div>
                  </el-col>
                  <el-col :span="16">
                  </el-col>
                </el-row>
              </el-tab-pane>
              <el-tab-pane label="抓取类型(爬虫-标识)" name="4">
                <el-row>
                  <el-col :span="5">
                    <el-form-item label="爬取量：">
                      <span style="color: red; font-size: 20px;">{{ formData.pv }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="预抓平均用时：">
                      <span style="color: red; font-size: 20px;">{{ formData.average+"ms"}}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="实时抓平均用时：">
                      <span style="color: red; font-size: 20px;">{{ formData.average1+"ms" }}</span>
                    </el-form-item>
                  </el-col>
                </el-row>
                <el-row :gutter="20" v-loading="loading">
                  <el-col :span="8">
                    <div id="main4"></div>
                  </el-col>
                  <el-col :span="16">
                  </el-col>
                </el-row>
              </el-tab-pane>
              <el-tab-pane label="爬虫-预抓数" name="5">
                <el-row>
                  <el-col :span="5">
                    <el-form-item label="爬取量：">
                      <span style="color: red; font-size: 20px;">{{ formData.pv }}</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="预抓平均用时：">
                      <span style="color: red; font-size: 20px;">{{ formData.average }}秒</span>
                    </el-form-item>
                  </el-col>
                  <el-divider direction="vertical"></el-divider>
                  <el-col :span="5">
                    <el-form-item label="实时抓平均用时：">
                      <span style="color: red; font-size: 20px;">{{ formData.average1 }}秒</span>
                    </el-form-item>
                  </el-col>
                </el-row>
                <el-row :gutter="20" v-loading="loading">
                  <el-col :span="8">
                    <div id="main5"></div>
                  </el-col>
                  <el-col :span="16">
                  </el-col>
                </el-row>
              </el-tab-pane>
            </el-tabs>
          </el-col>

        </el-row>
      </el-form>

      <el-row :gutter="20">
        <el-col :span="24">
          <div id="fx"></div>
        </el-col>
      </el-row>
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="base_Referer">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import * as echarts from 'echarts';

//import { formatDate } from '/@/utils/formatTime';
import { pageBase_Referer, getOption_Referer, list_Referer } from '/@/api/main/base_Referer';

type EChartsOption = echarts.EChartsOption;
const loading = ref(false);
const tabsRef = ref(null);
const tableData = ref<any>([]);
const queryParams = ref({
  ctime: 0,
});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const formData = ref({
  pv: 1,
  ipcount: 1,
  average:0,
  average1:0,
})


const activeName = ref('0')
const tabsValue = ref(0);
const rClick = (event: any) => {
  loading.value = true;
  queryParams.value.ctime = parseInt(event.target.value);
  initPieChart(tabsValue.value, queryParams.value.ctime, 'main' + tabsValue.value);
  initData(event.target.value, tabsValue.value);
  loading.value = false;
}
const handleClick = async (tab: any) => {
  loading.value = true;
  tabsValue.value = tab.props.name;
  initPieChart(tab.props.name, queryParams.value.ctime, 'main' + tab.props.name);
  initData(queryParams.value.ctime, tabsValue.value);
  loading.value = false;
}
// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageBase_Referer(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;



  loading.value = false;
};
const FirstQuery = async () => {
  const tInput: any = {
    timetype: 0,
  };
  var datares = await list_Referer(tInput);
  formData.value = datares.data.result;
}
// // 改变页面容量
// const handleSizeChange = (val: number) => {
//   tableParams.value.pageSize = val;
//   handleQuery();
// };

// // 改变页码序号
// const handleCurrentChange = (val: number) => {
//   tableParams.value.page = val;
//   handleQuery();
// };


//handleQuery();
FirstQuery();
onMounted(() => {
  setTimeout(() => {
    initPieChart(0, 0, 'main0');
  }, 700);
})
const initData = async (val: number, chatsType: number) => {

  //handleQuery();
  const tInput: any = {
    timetype: val,
    chatsType: chatsType,
  };
  var datares = await list_Referer(tInput);
  formData.value = datares.data.result;
}
const initPieChart = async (val: number, timetype: number, domId: string) => {
  loading.value = true;
  var chartDom = document.getElementById(domId)!;

  var myChart = echarts.init(chartDom);
  myChart.resize({ width: 1000, height: 700 });
  var option: EChartsOption;
  const tInput: any = {
    timetype: timetype,
    chatsType: val,
  };
  var res = await getOption_Referer(tInput);
  option = res.data.result;
  option && myChart.setOption(option, true);
  loading.value = false;
}
</script>


