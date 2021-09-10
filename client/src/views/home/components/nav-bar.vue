<template>
  <div class="nav flex flex-row items-center justify-center">
    <div class="container px-5 flex flex-row items-center justify-between">
      <span class="text-2xl text-gray-700">QNYD</span>
      <ul class="flex flex-row">
        <li class="text-sm ml-4 text-gray-700 flex-col items-center ml-4">
          <i class="text-blue-400 iconfont icon-view-grid" />
          <span class="ml-2">分类</span>
        </li>
        <li class="text-sm ml-4 text-gray-700 flex-col items-center ml-4">
          <i class="text-pink-400 iconfont icon-tag-multiple" />
          <span class="ml-2">标签</span>
        </li>
      </ul>
    </div>
  </div>
</template>
<script lang="ts" setup>
</script>
<style lang="scss" scoped>
.nav {
  height: 60px;
  width: 100%;
  position: fixed;
  top: 0;
  left: 0;
  transition: all 900ms cubic-bezier(0.23, 1, 0.32, 1) 0s;
  background-color: rgba(255, 255, 255, 0.4);
  backdrop-filter: blur(30px);
  opacity: 1;
  box-shadow: 0 2px 6px 0 rgb(0 0 0 / 6%), inset 0 -1px 0 0 #dadce0;
}
</style>
