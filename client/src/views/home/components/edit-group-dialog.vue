<template>
  <el-input v-model="name" size="mini" placeholder="请输入分组名称" />
</template>
<script lang="ts" setup>
import { ref, inject } from 'vue';
import { ElMessage, ElInput } from 'element-plus';
import { AfterConfirmEvent } from '@/components/basic-dialog/index';
import useApi from '@/api';

const name = ref<string>('');

const afterConfirm = inject('afterConfirm');
(afterConfirm as (args: AfterConfirmEvent) => void)(
  (hide, turnOffLoading, callback) => {
    if (!name.value) {
      ElMessage.error('名称不能为空');
      turnOffLoading();
    } else {
      const Apis = useApi();
      Apis.group.createGroup({
        name: name.value,
        tags: [],
      }).then(() => {
        ElMessage.success('创建成功');
        // eslint-disable-next-line no-unused-expressions
        callback && callback();
        hide();
      }).finally(turnOffLoading);
    }
  },
);
</script>
