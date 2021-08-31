<template>
  landing
  <el-button @click="handleToRegister">to register</el-button>
</template>
<script lang="ts">
import { defineComponent } from 'vue';
import { useRouter } from 'vue-router';

export default defineComponent({
  setup() {
    const router = useRouter();
    const handleToRegister = (): void => {
      router.push({
        name: 'login',
      });
    };
    return {
      handleToRegister,
    };
  },
});
</script>
