<template>
  <div class="nav flex flex-row items-center justify-center">
    <div class="container px-5 flex flex-row items-center justify-between h-full">
      <span class="text-xl text-gray-700">QNYD</span>
      <ul class="flex flex-row h-full">
        <el-dropdown>
          <li class="text-xs text-gray-600 flex flex-row items-center px-5 cursor-pointer h-full
          transition duration-600 ease-in-out bg-transparent hover:bg-blue-50 hover:text-blue-500">
            <i class="text-blue-400 iconfont icon-view-grid" />
            <span class="ml-1">Category</span>
          </li>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item v-for="group in groups" :key="group.id">
                {{ group.name }}
              </el-dropdown-item>
              <el-dropdown-item>
                <el-button
                  type="text" size="mini" @click="toggleEditGroupDialog">新建分组</el-button>
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
        <li class="text-xs text-gray-600 flex flex-row items-center px-5 cursor-pointer
          transition duration-600 ease-in-out bg-transparent hover:bg-blue-50 hover:text-blue-500">
          <i class="text-pink-400 iconfont icon-tag-multiple" />
          <span class="ml-1">Tags</span>
        </li>
        <li class="text-xs text-gray-600 flex flex-row items-center px-5 cursor-pointer
          transition duration-600 ease-in-out bg-transparent hover:bg-blue-50 hover:text-blue-500">
          <i class="text-gray-700 iconfont icon-github-circle" />
          <span class="ml-1">Github</span>
        </li>
      </ul>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ref, defineExpose } from 'vue';
import { useDialog } from '@/components/basic-dialog/index';
import EditGroupDialog from './edit-group-dialog.vue';
import useApi from '@/api';
import { GetGroupResponse, Group } from '@/api/modules/group';

const Apis = useApi();
const groups = ref<Array<Group>>([]);
const getGroups = () => {
  Apis.group.getGroup().then((res: GetGroupResponse) => {
    groups.value = res.data.entity;
  });
};
getGroups();

const toggleEditGroupDialog = () => {
  const { show } = useDialog({
    title: '创建分组',
    width: '400px',
    content: EditGroupDialog,
    callback: getGroups,
  });
  show();
};

defineExpose({ nav: '1' });

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
  box-shadow: 0 2px 6px 0 rgb(0 0 0 / 6%);
  z-index: 99;
}
</style>
