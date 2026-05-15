<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import Calendar from "primevue/calendar";
import { getFormErrors } from "@/core/utils/forms";

const props = defineProps<{
  id: string;
  validatorField$: any;
  submitted: boolean;
  label?: string;
  subLabel?: string;
  placeholder?: string;
}>();

const privateModel = ref<Date>(new Date());

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
  (e: "change", value: Date): void;
}>();

const handleDateSelect = (event: Date) => {
  emit("change", event);
};

const handleBlur = () => {
  emit("change", privateModel.value);
};

const errorMessages = computed<string>(() => {
  return getFormErrors(props.validatorField$);
});
</script>
<template>
  <div class="ves-form__field">
    <label v-if="props.label" :for="props.id">{{ props.label }}</label>
    <Calendar
      :id="props.id"
      :name="props.id"
      date-format="dd/mm/yy"
      :aria-describedby="`${props.id}-help`"
      v-model="privateModel"
      @date-select="handleDateSelect"
      @blur="handleBlur"
      :class="{ 'p-invalid': props.validatorField$.$invalid && submitted }"
      :placeholder="props.placeholder ?? ''"
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
