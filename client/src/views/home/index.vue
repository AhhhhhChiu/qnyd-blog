<template>
  <div class="bg-indigo-100">
    <nav-bar />
    <div v-for="i in 1000" :key="i">{{i}}</div>
  </div>
</template>
<script lang="ts" setup>
import NavBar from './components/nav-bar.vue';
</script>
