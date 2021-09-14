<template>
  <div class="bg-blue-50 flex flex-row justify-center items-center min-h-screen bg-">
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
<script setup lang="ts">
import { ref } from 'vue';
import Login from './components/login.vue';
import Register from './components/register.vue';

// eslint-disable-next-line no-shadow
enum VIEW {
  LOGIN, REGISTER,
}
const currentView = ref<VIEW>(VIEW.LOGIN);
const handleChangeView = (view: VIEW) => {
  currentView.value = view;
};
</script>
<style lang="scss" scoped>
::v-deep .button {
  transition: 0.3s all ease;
  padding: 10px;
  border-radius: 50%;
  background-color: #60A5FA;
  color: #fff;
  &:hover {
    transform: scale(10);
  }
  &:active {
    transform: scale(1);
  }
}
</style>
