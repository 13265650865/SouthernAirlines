<template>
  <div class="base_Airplane-container">
    <el-dialog v-model="isShowDialog" :title="props.title" width="950" draggable>
      <el-form
        :model="ruleForm"
        ref="ruleFormRef"
        size="default"
        label-width="100px"
        :rules="rules"
      >
        <el-row :gutter="35">
          <el-form-item v-show="false">
            <el-input v-model="ruleForm.id" />
          </el-form-item>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="所属机队" prop="planeFleetId">
              <el-select
                v-model="ruleForm.planeFleetId"
                filterable
                remote
                reserve-keyword
                clearable
                no-data-text="无机队数据"
                placeholder="请输入机队号"
                :remote-method="queryPlaneFleet"
                :loading="planeFleetLoading"
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
            <el-form-item label="注册号" prop="regisId">
              <el-input v-model="ruleForm.regisId" placeholder="请输入注册号" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="机型名" prop="planeModelName">
              <el-input
                v-model="ruleForm.planeModelName"
                placeholder="请输入机型名"
                clearable
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="机型号" prop="planeModelNo">
              <el-input
                v-model="ruleForm.planeModelNo"
                placeholder="请输入机型号"
                clearable
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="MSN" prop="msn">
              <el-input v-model="ruleForm.msn" placeholder="请输入MSN" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="VARTAB" prop="vartab">
              <el-input v-model="ruleForm.vartab" placeholder="请输入VARTAB" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="AMM_EFF" prop="ammEFF">
              <el-input v-model="ruleForm.ammEFF" placeholder="请输入AMM_EFF" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="IPC_EFF" prop="ipcEFF">
              <el-input v-model="ruleForm.ipcEFF" placeholder="请输入IPC_EFF" clearable />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label-width="0" prop="components">
              <el-table
                :data="ruleForm.components"
                style="width: 100%"
                tooltip-effect="light"
                row-key="id"
              >
                <el-table-column align="left" width="50" type="index">
                  <template #header>
                    <el-button
                      icon="el-icon-plus"
                      circle
                      @click="addComponentDetail"
                    ></el-button>
                  </template>
                </el-table-column>
                <el-table-column prop="partNum" label="部件件号" show-overflow-tooltip="">
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'components.' + scope.$index + '.componentId'"
                      :rules="rules.componentId"
                    >
                      <el-select
                        v-model="scope.row.componentId"
                        filterable
                        reserve-keyword
                        clearable
                        no-data-text="无飞机部件"
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
                <el-table-column prop="cmm" label="CMM" show-overflow-tooltip="">
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'components.' + scope.$index + '.cmm'"
                      :rules="rules.cmm"
                    >
                      <el-input
                        v-model="scope.row.cmm"
                        placeholder="请输入部件CMM"
                        clearable
                      />
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column prop="eff" label="EFF" show-overflow-tooltip="">
                  <template #default="scope">
                    <el-form-item
                      label-width="0"
                      :prop="'components.' + scope.$index + '.eff'"
                      :rules="rules.eff"
                    >
                      <el-input
                        v-model="scope.row.eff"
                        placeholder="请输入EFF"
                        clearable
                      />
                    </el-form-item>
                  </template>
                </el-table-column>
                <el-table-column
                  label="操作"
                  width="140"
                  align="center"
                  fixed="right"
                  show-overflow-tooltip=""
                >
                  <template #default="scope">
                    <el-button
                      icon="ele-Delete"
                      size="small"
                      text=""
                      type="primary"
                      @click.prevent="removeComponent(scope.$index)"
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
  addBase_Airplane,
  updateBase_Airplane,
  getPlane,
} from "/@/api/main/base_Airplane";
import { listPlaneFleet } from "/@/api/main/planeFleet";
import { pageComponent, listComponent } from "/@/api/main/component";
import { listComponentCategory } from "/@/api/main/componentCategory";

interface RuleFormDetail {
  id: number;
  componentId: number;
  cmm: string;
  eff: string;
}

interface RuleForm {
  id: number;
  regisId: string;
  planeModelName: string;
  planeModelNo: string;
  msn: string;
  vartab: string;
  ammEFF: string;
  ipcEFF: string;
  planeFleetId: number;
  planeFleetCode: string;
  components: Array<RuleFormDetail>;
}

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
const ruleForm = ref<RuleForm>({});
//自行添加其他规则
const rules = reactive<FormRules<RuleForm>>({
  regisId: [{ required: true, message: "请填写注册号", trigger: "blur" }],
  components: [{ required: false, message: "请提供替换件信息", trigger: "blur" }],
  componentId: [{ required: true, message: "请选择部件", trigger: "blur" }],
  cmm: [{ required: true, message: "请填写部件CMM", trigger: "blur" }],
});
const planeFleetLoading = ref(false);
const planeFleetOptions = ref<any>([]);

const componentCategoryLoading = ref(false);
const componentCategoryOptions = ref<any>([]);
const componentLoading = ref(false);
const componentOptions = ref<any>([]);

const queryComponent = async () => {
  componentLoading.value = true;
  var res = await listComponent({ type: 0 });
  componentOptions.value = res.data.result ?? [];
  componentLoading.value = false;
};

const queryComponentCategory = async (key) => {
  componentCategoryLoading.value = true;
  var res = await listComponentCategory({ name: key });
  componentCategoryOptions.value = res.data.result ?? [];
  componentCategoryLoading.value = false;
};

const queryPlaneFleet = async (key) => {
  planeFleetLoading.value = true;
  var res = await listPlaneFleet({ code: key });
  planeFleetOptions.value = res.data.result ?? [];
  planeFleetLoading.value = false;
};

const removeComponent = (index: number) => {
  ruleForm.value.components.splice(index, 1);
};

// 打开弹窗
const openDialog = async (row: any) => {
  if (row.id) {
    var res = await getPlane({
      id: row.id,
    });
    ruleForm.value = res.data.result;
  } else {
    ruleForm.value = {};
  }
  await queryComponent();
  isShowDialog.value = true;
};

// 关闭弹窗
const closeDialog = () => {
  emit("reloadTable");
  isShowDialog.value = false;
};

const addComponentDetail = () => {
  if (!ruleForm.value.components) {
    ruleForm.value.components = [];
  }
  ruleForm.value.components.push({
    id: null,
    componentId: null,
    cmm: null,
    eff: null,
  } as RuleFormDetail);
};

// 取消
const cancel = () => {
  isShowDialog.value = false;
};

// 提交
const submit = async () => {
  ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
    if (isValid) {
      let values = {
        id: ruleForm.value.id,
        regisId: ruleForm.value.regisId,
        planeModelName: ruleForm.value.planeModelName,
        planeModelNo: ruleForm.value.planeModelNo,
        msn: ruleForm.value.msn,
        vartab: ruleForm.value.vartab,
        ammEFF: ruleForm.value.ammEFF,
        ipcEFF: ruleForm.value.ipcEFF,
        planeFleetId: ruleForm.value.planeFleetId,
        components: ruleForm.value.components,
      };
      if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
        await updateBase_Airplane(values);
      } else {
        await addBase_Airplane(values);
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
onMounted(async () => {
  await queryPlaneFleet();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
