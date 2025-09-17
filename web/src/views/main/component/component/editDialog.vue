<template>
  <div class="component-container">
    <el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
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
          <el-form-item v-show="false">
            <el-input v-model="ruleForm.type" />
          </el-form-item>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="部件类别" prop="componentCategoryId">
              <el-select
                v-model="ruleForm.componentCategoryId"
                filterable
                remote
                reserve-keyword
                no-data-text="无部件类别"
                placeholder="请输入部件类别"
                :remote-method="queryComponentCategory"
                :loading="componentCategoryLoading"
              >
                <el-option
                  v-for="item in componentCategoryOptions"
                  :key="item.id"
                  :label="item.name"
                  :value="item.id"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="描述" prop="description">
              <el-input
                v-model="ruleForm.description"
                placeholder="请输入描述"
                clearable
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="件号" prop="partNum">
              <el-input v-model="ruleForm.partNum" placeholder="请输入件号" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="厂家" prop="factoryName">
              <el-input
                v-model="ruleForm.factoryName"
                placeholder="请输入厂家"
                clearable
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="数量" prop="quantity">
              <el-input-number
                v-model="ruleForm.quantity"
                placeholder="请输入数量"
                clearable
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="单位" prop="unit">
              <el-input v-model="ruleForm.unit" placeholder="请输入单位" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="备注" prop="remark">
              <el-input v-model="ruleForm.remark" placeholder="请输入备注" clearable />
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
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addComponent, updateComponent } from "/@/api/main/component";
import { listComponentCategory } from "/@/api/main/componentCategory";

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
const rules = ref<FormRules>({});
const editComponentTitle = ref("");
const componentCategoryLoading = ref(false);
const componentCategoryOptions = ref<any>([]);

const queryComponentCategory = async (key) => {
  componentCategoryLoading.value = true;
  var res = await listComponentCategory({ name: key });
  componentCategoryOptions.value = res.data.result ?? [];
  componentCategoryLoading.value = false;
};

// 打开弹窗
const openDialog = (row: any) => {
  ruleForm.value = JSON.parse(JSON.stringify(row));
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
        await updateComponent(values);
      } else {
        await addComponent(values);
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
  await queryComponentCategory();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
