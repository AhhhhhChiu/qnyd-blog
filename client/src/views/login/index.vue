<template>
  register/login
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
