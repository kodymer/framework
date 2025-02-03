<script setup lang="ts">
// Lib imports
import { ref, watch } from "vue";
import Dialog from "primevue/dialog";

// Component state (maybe necessary)
// const state = reactive<{
//   items: string[];
// }>({
//   items: [],
// });

// Reactive primitive values with ref (maybe necessary)
const visible = ref<boolean>(false);

// Properties
const props = defineProps<{
  visible: boolean;
  dismissable: boolean;
  header?: string;
}>();

// Watchers
watch(
  () => props.visible,
  () => {
    visible.value = props.visible;
  }
);

// Events
const emit = defineEmits<{
  (e: "toggle", visible: boolean): void;
}>();

// Event handlers
const handleChangeVisible = () => {
  emit("toggle", visible.value);
};
</script>

<template>
  <div>
    <Dialog
      v-model:visible="visible"
      :modal="true"
      :header="props?.header ?? ''"
      :dismissableMask="dismissable"
      @show="handleChangeVisible"
      @hide="handleChangeVisible"
    >
      <slot></slot>
    </Dialog>
  </div>
</template>

<style lang="scss" scoped>
// Scoped styles
</style>
