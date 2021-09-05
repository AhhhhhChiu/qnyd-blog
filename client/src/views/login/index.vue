<template>
  <div class="bg-indigo-100 flex flex-row justify-center items-center min-h-screen bg-">
    <div class="rounded-xl bg-white radius pt-20 pb-20 pl-32 pr-32
      flex flex-row justify-center items-center shadow-xl">
      <div class="h-96 w-auto">
        <img class="w-full h-full" src="/imgs/meat.jpg" alt="qnyd">
      </div>
      <login v-if="currentView === VIEW.LOGIN" @toRegister="handleChangeView(VIEW.REGISTER)" />
      <register v-if="currentView === VIEW.REGISTER" @toLogin="handleChangeView(VIEW.LOGIN)"/>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref } from 'vue';
import Login from './components/login.vue';
import Register from './components/register.vue';
import useApi from '@/api';

export default defineComponent({
  components: {
    Login, Register,
  },
  setup() {
    // const Api = useApi();
    // Api.user.getFlushKey().then((res) => {
    //   console.log('res: ', res);
    // });
    // eslint-disable-next-line no-shadow
    enum VIEW {
      LOGIN, REGISTER,
    }
    const currentView = ref<VIEW>(VIEW.LOGIN);
    const handleChangeView = (view: VIEW) => {
      currentView.value = view;
    };
    return {
      handleChangeView,
      currentView,
      VIEW,
    };
  },
});
</script>
<style lang="scss" scoped>
::v-deep .button {
  transition: 0.3s all ease;
  padding: 10px;
  border-radius: 50%;
  background-color: #818CF8;
  color: #fff;
  &:hover {
    transform: scale(10);
  }
  &:active {
    transform: scale(1);
  }
}
</style>
