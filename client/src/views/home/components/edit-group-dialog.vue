<template>
  <el-input v-model="name" size="mini" placeholder="请输入分组名称" />
</template>
<script lang="ts" setup>
import { ref, watch, defineProps } from 'vue';
import { ElMessage, ElInput } from 'element-plus';
import { useDialog } from '@/components/basic-dialog/index';
import useApi from '@/api';

const name = ref<string>('');

const props = defineProps({
  callback: {
    type: Function,
    default: () => ({}),
  },
});
const {
  confirmLoading, turnOffLoading, hide,
} = useDialog({});
watch(() => confirmLoading.value, (val) => {
  if (val) {
    if (!name.value) {
      ElMessage.error('名称不能为空');
      turnOffLoading();
    } else {
      // const createGroup
      const Apis = useApi();
      Apis.group.createGroup({
        name: name.value,
        tags: [],
      }).then(() => {
        ElMessage.success('创建成功');
        props.callback();
        hide();
      }).finally(turnOffLoading);
    }
  }
});
</script>
