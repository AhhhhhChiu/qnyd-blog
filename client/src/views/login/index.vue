<template>
  <div class="bg-indigo-100 flex flex-row justify-center items-center min-h-screen bg-">
    <div class="rounded-xl bg-white radius pt-20 pb-20 pl-32 pr-32
      flex flex-row justify-center items-center shadow-xl">
      <div class="h-96 w-auto">
        <img class="w-full h-full" src="/imgs/meat.jpg" alt="">
      </div>
      <div class="flex flex-col ml-40 w-72">
        <h1 class="pl-1 text-2xl font-semibold text-gray-600 mb-2">BLOG</h1>
        <span class="pl-1 text-sm text-gray-400 mb-12">Qnyd blog developed with song jeam</span>
        <input class="mb-6 border h-11 rounded-xl box-border w-full pl-6 pr-6
        focus:outline-none placeholder-gray-500
        placeholder-opacity-25 text-sm"
          type="text" placeholder="Account" />
        <input
          class="mb-12 border h-11 rounded-xl box-border w-full pl-6 pr-6
          focus:outline-none placeholder-gray-500
          placeholder-opacity-25 text-sm"
          type="password" placeholder="Password" />
        <div>
          <span class="text-indigo-400 ml-1 text-xl el-icon-s-promotion"></span>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue';
import useApi from '@/api';

export default defineComponent({
  setup() {
    const Api = useApi();
    Api.user.getFlushKey().then((res) => {
      console.log('res: ', res);
    });
  },
});
</script>
<style lang="scss" scoped>
.card {

}
</style>
