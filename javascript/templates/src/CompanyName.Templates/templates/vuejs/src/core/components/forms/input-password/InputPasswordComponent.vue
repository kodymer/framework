<script setup lang="ts">
import { computed } from "vue";
import Password from "primevue/password";
import { getFormErrors } from "@/core/utils/forms";

const props = defineProps<{
  id: string;
  validatorField$: any;
  submitted: boolean;
  label?: string;
  subLabel?: string;
  placeholder?: string;
  feedback?: boolean;
}>();

const errorMessages = computed<string>(() => {
  return getFormErrors(props.validatorField$);
});

const emit = defineEmits<{
  (e: "change", value: string): void;
}>();

const handleInput = (event: any) => {
  emit("change", event.target.value);
};
</script>
<template>
  <div class="ves-form__field">
    <label v-if="props.label" :for="props.id">{{ props.label }}</label>
    <Password
      :id="props.id"
      :name="props.id"
      :feedback="props.feedback"
      :toggleMask="true"
      :aria-describedby="`${props.id}-help`"
      :value="props.validatorField$.$model"
      @input="handleInput"
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
