<template>
  <div class="flex flex-col ml-40 w-72">
    <h1 class="pl-1 text-2xl font-semibold text-gray-600 mb-2">BLOG</h1>
    <span class="pl-1 text-sm text-gray-400 mb-12">Qnyd blog developed with song jeam</span>
    <input class="mb-6 border h-11 rounded-xl box-border w-full pl-6 pr-6
      focus:outline-none placeholder-gray-500
      placeholder-opacity-25 text-sm"
      v-model="form.userName"
      type="text" placeholder="Account" />
    <input
      class="mb-6 border h-11 rounded-xl box-border w-full pl-6 pr-6
      focus:outline-none placeholder-gray-500
      placeholder-opacity-25 text-sm"
      v-model="form.passwordHash"
      type="password" placeholder="Password" />
    <div class="mb-10 pl-1 text-blue-400 text-xs cursor-pointer">
      <span @click="handleToRegister">还没有账号？qnyd！</span>
    </div>
    <div>
      <span
        :class="loading ? 'el-icon-loading' : 'el-icon-s-promotion'" @click="handleLogin"
        class="button cursor-pointer text-indigo-400 ml-1 text-xl" />
    </div>
  </div>
</template>
<script lang="ts">
import { ElMessage } from 'element-plus';
import { defineComponent } from 'vue';
import { useLanding } from '../hooks/useLanding';

export default defineComponent({
  setup(_, context) {
    /** 跳转 */
    const handleToRegister = (): void => {
      context.emit('toRegister');
    };

    /** 登录 */
    const {
      handleSubmit: handleLogin, loading, form,
    } = useLanding('register', () => {
      ElMessage.success('登录成功');
    });

    return {
      handleLogin,
      loading,
      form,
      handleToRegister,
    };
  },
});
</script>
