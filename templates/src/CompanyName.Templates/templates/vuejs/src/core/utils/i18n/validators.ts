import * as validators from "@vuelidate/validators";
import i18n from "@/config/i18n";

const { createI18nMessage } = validators;
const { t } = i18n.global;

const withI18nMessage = createI18nMessage({ t });

// Required rule
export const required = withI18nMessage(validators.required);

// Min length rule
export const minLength = withI18nMessage(validators.minLength, {
  withArguments: true,
});

// Checked rule (for checkbox controls)
const checkedValidator = (value: any) => value === true;
export const checked = validators.helpers.withMessage(
  t("validations.checked"),
  checkedValidator
);

// Any check rule (for checkbox group controls)
export const anyChecked = validators.helpers.withMessage(
  t("validations.anyChecked"),
  required
);
