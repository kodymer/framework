<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import Checkbox from "primevue/checkbox";
import { getFormErrors } from "@/core/utils/forms";

const props = defineProps<{
  id: string;
  validatorField$: any;
  submitted: boolean;
  label: string;
  subLabel?: string;
}>();

const privateModel = ref(false);

onMounted(() => {
  privateModel.value = props.validatorField$.$model;
});

watch(
  () => props.validatorField$.$model,
  () => {
    privateModel.value = props.validatorField$.$model;
  }
);

const emit = defineEmits<{
  (e: "change", value: boolean): void;
}>();

const handleInput = (value: any) => {
  emit("change", value as boolean);
};

const errorMessages = computed<string>(() => {
  return getFormErrors(props.validatorField$);
});
</script>
<template>
  <div class="field-checkbox ves-form__field">
    <Checkbox
      :id="props.id"
      :name="props.id"
      v-model="privateModel"
      :aria-describedby="`${props.id}-help`"
      :binary="true"
      :class="{ 'p-invalid': props.validatorField$.$invalid && submitted }"
      @input="handleInput"
    />
    <label :for="props.id">{{ props.label }}</label>
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
