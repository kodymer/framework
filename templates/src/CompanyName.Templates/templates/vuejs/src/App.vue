<script setup lang="ts">
import HeaderComponent from "@/core/components/ui/header/HeaderComponent.vue";
import FooterComponent from "@/core/components/ui/footer/FooterComponent.vue";
</script>

<template>
  <HeaderComponent />
  <RouterView />
  <FooterComponent />
</template>

<style lang="scss">
@import "primevue/resources/primevue.min.css";
@import "primeicons/primeicons.css";
@import "primevue/resources/themes/lara-light-blue/theme.css";
@import "@/assets/base";
</style>
