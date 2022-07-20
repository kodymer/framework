<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import RadioButton from "primevue/radiobutton";
import { getFormErrors } from "@/core/utils/forms";
import type { IntFormSelectItem } from "./../types/IntFormSelectItem";

const props = defineProps<{
  id: string;
  validatorField$: any;
  value: any;
  submitted: boolean;
  options: IntFormSelectItem[];
  label?: string;
  subLabel?: string;
}>();

const privateModel = ref(null);

onMounted(() => {
  privateModel.value = props.value;
});

watch(
  () => props.value,
  () => {
    privateModel.value = props.value;
  }
);

const emit = defineEmits<{
  (e: "change", value: any): void;
}>();

const handleClick = (value: any) => {
  emit("change", value);
};

const errorMessages = computed<string>(() => {
  return getFormErrors(props.validatorField$);
});
</script>
<template>
  <div class="ves-form__field">
    <label v-if="props.label" :for="props.id">{{ props.label }}</label>
    <div
      v-for="(option, index) in options"
      :key="index"
      class="field-radiobutton"
    >
      <RadioButton
        :id="`${props.id}${index}`"
        :name="props.id"
        :value="option.value"
        v-model="privateModel"
        :class="{ 'p-invalid': props.validatorField$.$invalid && submitted }"
        @click="handleClick(option.value)"
      />
      <label :for="`${props.id}${index}`">{{ option.label }}</label>
    </div>
    <small v-if="!!props.subLabel" :id="`${props.id}-help`">{{
      props.subLabel
    }}</small>
    <small
      v-if="
        (props.validatorField$.$invalid && submitted) ||
        props.validatorField$.$pending
      "
      class="p-error"
      v-html="errorMessages"
    ></small>
  </div>
</template>
