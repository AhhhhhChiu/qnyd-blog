<template>
  <div class="flex flex-col ml-40 w-72">
    <h1 class="pl-1 text-2xl font-semibold text-gray-600 mb-2">新账号</h1>
    <span class="pl-1 text-sm text-gray-400 mb-12">注册新账号开始 qnyd</span>
    <input class="mb-6 border h-11 rounded-xl box-border w-full pl-6 pr-6
      focus:outline-none placeholder-gray-500 placeholder-opacity-25 text-sm"
      v-model="form.userName" type="text" placeholder="Account" />
    <input class="mb-6 border h-11 rounded-xl box-border w-full pl-6 pr-6
      focus:outline-none placeholder-gray-500 placeholder-opacity-25 text-sm"
      v-model="form.passwordHash" @keypress.enter="handleRegister"
      type="password" placeholder="Password" />
    <div class="mb-10 pl-1 text-blue-400 text-xs cursor-pointer">
      <span @click="handleToLogin">已经有账号了？qnyd！</span>
    </div>
    <div>
      <span
        :class="loading ? 'el-icon-loading' : 'el-icon-check'" @click="handleRegister"
        class="button cursor-pointer text-indigo-400 ml-1 text-xl" />
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ElMessage } from 'element-plus';
import { defineEmits } from 'vue';
import { useLanding } from '../hooks/useLanding';

/** 跳转 */
const emit = defineEmits<{(e: 'toLogin'): void}>();
const handleToLogin = (): void => {
  emit('toLogin');
};

/** 注册 */
const {
  handleSubmit: handleRegister, loading, form,
} = useLanding('register', () => {
  ElMessage.success('注册成功');
  handleToLogin();
});
</script>
