<script setup lang="ts">
import { ref, reactive } from "vue";
import Button from "primevue/button";
import { useVuelidate } from "@vuelidate/core";
import { required, minLength } from "@/core/utils/i18n/validators";
import $t from "@/core/utils/i18n/translate";

import InputTextComponent from "@/core/components/forms/input-text/InputTextComponent.vue";
import InputPasswordComponent from "@/core/components/forms/input-password/InputPasswordComponent.vue";

const submitted = ref(false);

const formState = reactive<{
  username: string;
  password: string;
}>({
  username: "",
  password: "",
});

const formRules = {
  username: { required, minLength: minLength(4) },
  password: { required },
};

const validation$ = useVuelidate(formRules, formState);

const handleSubmit = (isFormValid: boolean) => {
  submitted.value = true;
  if (isFormValid) {
    alert("Login !!!");
  }
};
</script>

<template>
  <div class="ves-form">
    <form @submit.prevent="handleSubmit(!validation$.$invalid)">
      <!-- Username -->
      <InputTextComponent
        id="username"
        :label="$t('login.username')"
        :placeholder="$t('login.enter_username')"
        :validator-field$="validation$.username"
        :submitted="submitted"
        @change="(value) => (formState.username = value)"
      />
      <!-- Password -->
      <InputPasswordComponent
        id="password"
        :label="$t('login.password')"
        :placeholder="$t('login.enter_password')"
        :validator-field$="validation$.password"
        :submitted="submitted"
        @change="(value) => (formState.password = value)"
      />
      <div class="ves-form__buttons">
        <Button type="submit" :label="$t('login.enter')" />
      </div>
    </form>
  </div>
</template>
