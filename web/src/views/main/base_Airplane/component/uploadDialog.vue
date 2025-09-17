<template>
  <div class="base_Airplane-container">
    <el-dialog v-model="isShowUploadDialog" title="导入机型部件" :width="450" draggable>
      <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
        <el-form
          ref="uploadFormRef"
          :model="uploadForm"
          size="default"
          label-width="100px"
          :rules="uploadRules"
        >
          <el-form-item label="机队" prop="planeFleetId">
            <el-select
              v-model="uploadForm.planeFleetId"
              filterable
              remote
              reserve-keyword
              no-data-text="无机队数据"
              placeholder="请输入机队号"
              :remote-method="queryPlaneFleet"
              :loading="planeFleetLoading"
              @change="queryPlane"
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
          <el-form-item label="飞机注册号" prop="planeId">
            <el-select
              v-model="uploadForm.planeId"
              filterable
              clearable
              reserve-keyword
              no-data-text="无机队数据"
              placeholder="请输入注册号"
              :loading="planeLoading"
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
          <el-form-item>
            <el-row>
              <el-col :span="12">
                <el-button
                  type="primary"
                  icon="ele-Download"
                  @click="getPlaneUploadTemplate"
                  v-auth="'planeFleet:add'"
                >
                  导入模板
                </el-button>
              </el-col>
              <el-col :span="12">
                <el-upload
                  ref="uploadRef"
                  accept=".xlsx"
                  :action="uploadUrl"
                  :headers="authHeaders"
                  style="margin-left: 12px"
                  :before-upload="beforeAvatarUpload"
                  :data="uploadForm"
                  v-loading.fullscreen.lock="fullscreenLoading"
                  :on-error="closeLoading"
                  :on-progress="showLoading"
                  :on-success="uploadSuccess"
                >
                  <el-button type="primary" icon="ele-UploadFilled">导入</el-button>
                </el-upload>
              </el-col>
            </el-row>
          </el-form-item>
        </el-form>
      </el-card>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="cancel" size="default">取 消</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup="" name="planeComponentUpload">
import { ref, FormInstance, UploadInstance, reactive, onMounted } from "vue";
import type { FormRules, UploadProps } from "element-plus";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from "/@/utils/authFunction";
import { getToken } from "/@/utils/axios-utils";
import { listPlaneFleet } from "/@/api/main/planeFleet";
import { listPlane } from "/@/api/main/base_Airplane";
import { getUploadTemplatePlane } from "/@/api/main/base_Airplane";

const emit = defineEmits(["reloadTable"]);
const uploadFormRef = ref<FormInstance>();
const uploadRef = ref<UploadInstance>();
const uploadForm = ref<any>({});
const isShowUploadDialog = ref(false);
const authHeaders = ref<any>({
  Authorization: `Bearer ${getToken()}`,
});
const uploadUrl = ref(`${import.meta.env.VITE_API_URL}/api/plane/upload`);
const fullscreenLoading = ref(false);
const showLoading = () => {
  fullscreenLoading.value = true;
};
const closeLoading = () => {
  fullscreenLoading.value = false;
};
const uploadSuccess = async (res) => {
  if (res.code == 200) {
    ElMessage({
      message: "导入成功",
      type: "info",
    });
  } else {
    ElMessage({
      message: res.message,
      type: "error",
    });
  }
  fullscreenLoading.value = false;
};

const planeFleetLoading = ref(false);
const planeFleetOptions = ref<any>([]);
const planeLoading = ref(false);
const planeOptions = ref<any>([]);

const uploadRules = reactive<FormRules<RuleForm>>({
  planeFleetId: [{ required: true, message: "请选择机队", trigger: "blur" }],
});

const queryPlaneFleet = async (key) => {
  planeFleetLoading.value = true;
  var res = await listPlaneFleet({ code: key });
  planeFleetOptions.value = res.data.result ?? [];
  planeFleetLoading.value = false;
};

const queryPlane = async (value) => {
  planeLoading.value = true;
  var res = await listPlane({
    planeFleetId: value,
  });
  planeOptions.value = res.data.result ?? [];
  planeLoading.value = false;
};

const beforeAvatarUpload: UploadProps["beforeUpload"] = async (rawFile) => {
  var valid = await uploadFormRef.value.validate();
  if (valid) {
    return true;
  }
  return false;
};

const getPlaneUploadTemplate = () => {
  getUploadTemplatePlane().then((res) => {
    const { data, headers } = res;
    const blob = new Blob([data], { type: headers["content-type"] });
    let dom = document.createElement("a");
    let url = window.URL.createObjectURL(blob);
    dom.href = url;
    dom.download = "PlaneComponent.xlsx";
    dom.style.display = "none";
    document.body.appendChild(dom);
    dom.click();
    dom.parentNode.removeChild(dom);
    window.URL.revokeObjectURL(url);
  });
};

const openDialog = async () => {
  isShowUploadDialog.value = true;
};

const cancel = () => {
  isShowUploadDialog.value = false;
};

onMounted(async () => {
  await queryPlaneFleet();
});

defineExpose({ openDialog });
</script>
