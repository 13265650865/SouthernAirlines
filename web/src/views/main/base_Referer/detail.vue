<template>
    <div class="detail-container">
        <el-tabs ref="tabsRef" v-model="activeName" @tab-click="handleClick">
            <el-tab-pane label="酒店访问次数明细" name="0">
                <el-card shadow="hover" header="">
                    <el-form :model="queryParams" ref="queryForm" :inline="true">
                        <el-form-item label="开始时间" prop="name">
                            <el-date-picker v-model="queryParams.startTime" type="datetime" placeholder="开始时间"
                                value-format="YYYY-MM-DD HH:mm:ss" />
                        </el-form-item>
                        <el-form-item label="结束时间" prop="code">
                            <el-date-picker v-model="queryParams.endTime" type="datetime" placeholder="结束时间"
                                value-format="YYYY-MM-DD HH:mm:ss" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询
                            </el-button>
                        </el-form-item>
                    </el-form>
                </el-card>
                <el-card class="full-table" shadow="hover" style="margin-top: 8px">
                    <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id"
                        border="">
                        <el-table-column prop="name" label="酒店Id" fixed="" show-overflow-tooltip="" />
                        <el-table-column prop="value" label="访问次数" fixed="" show-overflow-tooltip="" />
                    </el-table>
                    <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
                        :total="tableParams.total" :page-sizes="[50, 100]" small="" background=""
                        @size-change="handleSizeChange" @current-change="handleCurrentChange"
                        layout="total, sizes, prev, pager, next, jumper" />
                    <editDialog ref="editDialogRef" :title="editBase_SearchlogTitle" @reloadTable="handleQuery" />
                </el-card>
            </el-tab-pane>
            <el-tab-pane label="预抓酒店数量" name="1">
                <el-card shadow="hover" header="">
                    <el-form :model="queryParams" ref="queryForm" :inline="true">
                        <el-form-item label="开始时间" prop="name">
                            <el-date-picker v-model="queryParams.startTime" type="datetime" placeholder="开始时间"
                                value-format="YYYY-MM-DD HH:mm:ss" />
                        </el-form-item>
                        <el-form-item label="结束时间" prop="code">
                            <el-date-picker v-model="queryParams.endTime" type="datetime" placeholder="结束时间"
                                value-format="YYYY-MM-DD HH:mm:ss" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询
                            </el-button>
                        </el-form-item>
                    </el-form>
                </el-card>
                <el-card class="full-table" shadow="hover" style="margin-top: 8px">
                    <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id"
                        border="">
                        <el-table-column prop="name" label="预抓数量" fixed="" show-overflow-tooltip="" />
                        <el-table-column prop="value" label="Count" fixed="" show-overflow-tooltip="" />
                    </el-table>
                    <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
                        :total="tableParams.total" :page-sizes="[50, 100]" small="" background=""
                        @size-change="handleSizeChange" @current-change="handleCurrentChange"
                        layout="total, sizes, prev, pager, next, jumper" />
                    <editDialog ref="editDialogRef" :title="editBase_SearchlogTitle" @reloadTable="handleQuery" />
                </el-card>
            </el-tab-pane>
            <el-tab-pane label="IP地区数据明细" name="2">
                <el-card shadow="hover" header="">
                    <el-form :model="queryParams" ref="queryForm" :inline="true">
                        <el-form-item label="开始时间" prop="name">
                            <el-date-picker v-model="queryParams.startTime" type="datetime" placeholder="开始时间"
                                value-format="YYYY-MM-DD HH:mm:ss" />
                        </el-form-item>
                        <el-form-item label="结束时间" prop="code">
                            <el-date-picker v-model="queryParams.endTime" type="datetime" placeholder="结束时间"
                                value-format="YYYY-MM-DD HH:mm:ss" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询
                            </el-button>
                        </el-form-item>
                    </el-form>
                </el-card>
                <el-card class="full-table" shadow="hover" style="margin-top: 8px">
                    <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id"
                        border="">
                        <el-table-column prop="name" label="预抓数量" fixed="" show-overflow-tooltip="" />
                        <el-table-column prop="value" label="Count" fixed="" show-overflow-tooltip="" />
                    </el-table>
                    <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
                        :total="tableParams.total" :page-sizes="[50, 100]" small="" background=""
                        @size-change="handleSizeChange" @current-change="handleCurrentChange"
                        layout="total, sizes, prev, pager, next, jumper" />
                    <editDialog ref="editDialogRef" :title="editBase_SearchlogTitle" @reloadTable="handleQuery" />
                </el-card>
            </el-tab-pane>
        </el-tabs>
    </div>
</template>
<script lang="ts" setup="" name="base_detail">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { hotel_detail } from '/@/api/main/base_Referer';

const activeName = ref('0')
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

const handleClick = async () => {

}
// 查询操作
const handleQuery = async () => {
    loading.value = true;
    var res = await hotel_detail(Object.assign(queryParams.value, tableParams.value));
    tableData.value = res.data.result ?? [];
    console.log(tableData.value)
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


handleQuery();
</script>