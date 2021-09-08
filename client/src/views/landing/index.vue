<template>
  landing
  <el-button @click="handleToRegister">to register</el-button>
</template>
<script lang="ts" setup>
import { useRouter } from 'vue-router';

const router = useRouter();
const handleToRegister = (): void => {
  router.push({
    name: 'login',
  });
};
</script>
