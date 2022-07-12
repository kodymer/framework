<script setup lang="ts">
import { computed } from "vue";
import Dropdown from "primevue/dropdown";
import { getFormErrors } from "@/core/utils/forms";
import type { IntFormSelectItem } from "./../types/IntFormSelectItem";

const props = defineProps<{
  id: string;
  validatorField$: any;
  submitted: boolean;
  options: IntFormSelectItem[];
  label?: string;
  subLabel?: string;
  placeholder?: string;
}>();

const emit = defineEmits<{
  (e: "change", value: any): void;
}>();

const handleChange = (event: any) => {
  emit("change", event.value);
};

const errorMessages = computed<string>(() => {
  return getFormErrors(props.validatorField$);
});
</script>
<template>
  <div class="ves-form__field">
    <label v-if="props.label" :for="props.id">{{ props.label }}</label>
    <Dropdown
      :inputId="props.id"
      :name="props.id"
      :aria-labelled-by="`${props.id}-help`"
      :modelValue="props.validatorField$.$model"
      :options="props.options"
      optionLabel="label"
      optionValue="value"
      :placeholder="props.placeholder ?? ''"
      :class="{ 'p-invalid': props.validatorField$.$invalid && submitted }"
      @change="handleChange"
    />
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
