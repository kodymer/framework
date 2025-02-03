<script setup lang="ts">
import { computed, onMounted, ref, watch, type Ref } from "vue";
import Checkbox from "primevue/checkbox";
import type { IntFormSelectItem } from "./../types/IntFormSelectItem";
import { getFormErrors } from "@/core/utils/forms";

const props = defineProps<{
  id: string;
  validatorField$: any;
  value: any[];
  submitted: boolean;
  options: IntFormSelectItem[];
  label?: string;
  subLabel?: string;
}>();

const privateModel: Ref<any[]> = ref([]);

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

const handleInput = (value: any) => {
  if (!props.value) {
    emit("change", [value]);
  } else if (props.value.indexOf(value) > -1) {
    emit(
      "change",
      props.value.filter((item) => item !== value)
    );
  } else {
    emit("change", [...props.value, value]);
  }
};

const errorMessages = computed<string>(() => {
  return getFormErrors(props.validatorField$);
});
</script>
<template>
  <div class="ves-form__field">
    <label v-if="props.label" :for="props.id">{{ props.label }}</label>
    <div v-for="(option, index) in options" :key="index" class="field-checkbox">
      <Checkbox
        :id="`${props.id}${index}`"
        :name="props.id"
        :value="option.value"
        v-model="privateModel"
        :aria-describedby="`${props.id}-help`"
        :class="{ 'p-invalid': props.validatorField$.$invalid && submitted }"
        @input="handleInput(option.value)"
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
