<template>
  <div class="planeComponentDefectRecord-container">
    <el-dialog v-model="isShowDialog" :title="props.title" fullscreen draggable="">
      <el-form
        :model="ruleForm"
        ref="ruleFormRef"
        size="default"
        label-width="140px"
        :rules="rules"
      >
        <el-row :gutter="35">
          <el-form-item v-show="false">
            <el-input v-model="ruleForm.id" />
          </el-form-item>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="所属机队">
              <el-select
                v-model="ruleForm.planeFleetId"
                filterable
                remote
                reserve-keyword
                no-data-text="无机队数据"
                placeholder="请输入机队号"
                :remote-method="queryPlaneFleet"
                :loading="planeFleetLoading"
                @change="clearPlane"
              >
                <el-option
                  v-for="item in planeFleetOptions"
                  :key="item.id"
                  :label="item.code"
                  :value="item.id"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="飞机注册号" prop="planeId">
              <el-select
                v-model="ruleForm.planeId"
                filterable
                remote
                reserve-keyword
                no-data-text="无机队数据"
                placeholder="请输入注册号"
                :remote-method="queryPlane"
                :loading="planeLoading"
                @change="queryPlaneComponent"
              >
                <el-option
                  v-for="item in planeOptions"
                  :key="item.id"
                  :label="item.regisId"
                  :value="item.id"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="部件" prop="componentId">
              <el-select
                v-model="ruleForm.componentId"
                filterable
                reserve-keyword
                no-data-text="无关联部件"
                placeholder="请输入部件件号"
              >
                <el-option
                  v-for="item in planeComponentOptions"
                  :key="item.id"
                  :label="item.partNum"
                  :value="item.id"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="CMM" prop="cmm">
              <el-input v-model="ruleForm.cmm" clearable placeholder="请输入部件CMM" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="部件中文缺陷描述" prop="chineseDefectDescription">
              <el-input
                v-model="ruleForm.chineseDefectDescription"
                placeholder="请输入部件中文缺陷描述"
                type="textarea"
                clearable
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label-width="0" prop="details">
              <el-table :data="ruleForm.details">
                <el-table-column type="index" width="60" align="left">
                  <template #header>
                    <el-button icon="el-icon-plus" circle @click="addDetail"></el-button>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="replaceComponentPartNum"
                  label="替换部件件号"
                  show-overflow-tooltip
                >
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'details.' + scope.$index + '.replaceComponentId'"
                      :rules="rules.replaceComponentId"
                    >
                      <el-select
                        v-model="scope.row.replaceComponentId"
                        filterable
                        reserve-keyword
                        clearable
                        no-data-text="无缺陷部件"
                        placeholder="请选择部件"
                      >
                        <el-option
                          v-for="item in componentOptions"
                          :key="item.id"
                          :label="item.partNum"
                          :value="item.id"
                        >
                        </el-option>
                      </el-select>
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="componenDescription"
                  label="部件描述"
                  show-overflow-tooltip
                >
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'details.' + scope.$index + '.componenDescription'"
                      :rules="rules.componenDescription"
                    >
                      <el-input
                        v-model="scope.row.componenDescription"
                        placeholder="请输入部件描述"
                        type="textarea"
                        size="small"
                        clearable
                      />
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="englishDefectDescription"
                  label="部件英文缺陷描述"
                  show-overflow-tooltip
                >
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'details.' + scope.$index + '.englishDefectDescription'"
                      :rules="rules.englishDefectDescription"
                    >
                      <el-input
                        v-model="scope.row.englishDefectDescription"
                        placeholder="请输入部件英文缺陷描述"
                        type="textarea"
                        size="small"
                        clearable
                      />
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column prop="quantity" label="数量" show-overflow-tooltip>
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'details.' + scope.$index + '.quantity'"
                      :rules="rules.quantity"
                    >
                      <el-input-number
                        v-model="scope.row.quantity"
                        placeholder="请输入数量"
                        class="mx-4"
                        min="1"
                        controls-position="right"
                        size="small"
                        clearable
                      />
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column prop="unit" label="单位" show-overflow-tooltip>
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'details.' + scope.$index + '.unit'"
                      :rules="rules.unit"
                    >
                      <el-input
                        v-model="scope.row.unit"
                        placeholder="请输入单位"
                        size="small"
                        clearable
                      />
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column
                  label="操作"
                  align="center"
                  fixed="right"
                  show-overflow-tooltip
                >
                  <template #default="scope">
                    <el-button
                      icon="ele-Delete"
                      size="small"
                      text=""
                      type="primary"
                      @click.prevent="removeDetail(scope.$index)"
                    >
                      删除
                    </el-button>
                  </template>
                </el-table-column>
              </el-table>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="cancel" size="default">取 消</el-button>
          <el-button type="primary" @click="submit" size="default">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import {
  addPlaneComponentDefectRecord,
  updatePlaneComponentDefectRecord,
} from "/@/api/main/planeComponentDefectRecord";
import { listPlaneFleet } from "/@/api/main/planeFleet";
import { listPlane, getPlane } from "/@/api/main/base_Airplane";
import { listComponent } from "/@/api/main/component";

interface RuleFormDetail {
  id: number;
  englishDefectDescription: string;
  componenDescription: string;
  replaceComponentId: string;
  quantity: number;
  unit: string;
}

interface RuleForm {
  id: number;
  planeFleetId: string;
  planeId: number;
  componentId: number;
  cmm: string;
  chineseDefectDescription: string;
  details: Array<RuleFormDetail>;
}

const clearPlane = () => {
  ruleForm.value.planeId = null;
};

const addDetail = () => {
  if (!ruleForm.value.details) {
    ruleForm.value.details = [];
  }
  ruleForm.value.details.push({
    id: null,
    englishDefectDescription: null,
    componenDescription: null,
    replaceComponentId: null,
    quantity: 1,
    unit: null,
  } as RuleFormDetail);
};

const removeDetail = (index: number) => {
  ruleForm.value.details.splice(index, 1);
};

const planeFleetLoading = ref(false);
const planeFleetOptions = ref<any>([]);

const queryPlaneFleet = async (key) => {
  planeFleetLoading.value = true;
  var res = await listPlaneFleet({ code: key });
  planeFleetOptions.value = res.data.result ?? [];
  planeFleetLoading.value = false;
};

const planeLoading = ref(false);
const planeOptions = ref<any>([]);

const queryPlane = async (key) => {
  planeLoading.value = true;
  var res = await listPlane({
    planeFleetId: ruleForm.value.planeFleetId,
    regisId: key,
  });
  planeOptions.value = res.data.result ?? [];
  planeLoading.value = false;
};

const planeComponentLoading = ref(false);
const planeComponentOptions = ref<any>([]);

const queryPlaneComponent = async () => {
  planeComponentLoading.value = true;
  if (ruleForm.value.planeId) {
    var res = await getPlane({
      id: ruleForm.value.planeId,
    });
    planeComponentOptions.value = res.data.result.components ?? [];
  } else {
    planeComponentOptions.value = [];
  }
  planeComponentLoading.value = false;
};

const componentLoading = ref(false);
const componentOptions = ref<any>([]);

const queryComponent = async () => {
  componentLoading.value = true;
  var res = await listComponent({ type: 1 });
  componentOptions.value = res.data.result ?? [];
  componentLoading.value = false;
};

//父级传递来的参数
var props = defineProps({
  title: {
    type: String,
    default: "",
  },
});
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});
//自行添加其他规则
const rules = reactive<FormRules<RuleForm>>({
  planeId: [{ required: true, message: "请选择飞机", trigger: "blur" }],
  cmm: [{ required: true, message: "请填写CMM", trigger: "blur" }],
  componentId: [{ required: true, message: "请选择部件", trigger: "blur" }],
  chineseDefectDescription: [
    { required: true, message: "请填写中文缺陷描述", trigger: "blur" },
  ],
  details: [{ required: true, message: "请提供替换件信息", trigger: "blur" }],
  replaceComponentId: [{ required: true, message: "请填选择替换件", trigger: "blur" }],
  englishDefectDescription: [
    { required: true, message: "请填写英文缺陷描述", trigger: "blur" },
  ],
  componenDescription: [{ required: true, message: "请填写部件描述", trigger: "blur" }],
  quantity: [{ required: true, message: "请填写替换件数量", trigger: "blur" }],
  unit: [{ required: false, message: "请填写单位", trigger: "blur" }],
});

// 打开弹窗
const openDialog = async (row: any) => {
  ruleForm.value = JSON.parse(JSON.stringify(row));
  await queryPlaneFleet();
  await queryPlane();
  await queryPlaneComponent();
  await queryComponent();
  isShowDialog.value = true;
};

// 关闭弹窗
const closeDialog = () => {
  emit("reloadTable");
  isShowDialog.value = false;
};

// 取消
const cancel = () => {
  isShowDialog.value = false;
};

// 提交
const submit = async () => {
  ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
    if (isValid) {
      let values = ruleForm.value;
      if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
        await updatePlaneComponentDefectRecord(values);
      } else {
        await addPlaneComponentDefectRecord(values);
      }
      closeDialog();
    } else {
      ElMessage({
        message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
        type: "error",
      });
    }
  });
};

// 页面加载时
onMounted(async () => {});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
