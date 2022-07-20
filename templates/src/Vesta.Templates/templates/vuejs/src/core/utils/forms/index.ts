export const getFormErrors = (validatorField: any): string => {
  const messages: string[] = [];
  if (validatorField.required?.$invalid) {
    messages.push(validatorField.required.$message);
  }
  if (validatorField.minLength?.$invalid) {
    messages.push(validatorField.minLength.$message);
  }
  if (validatorField.checked?.$invalid) {
    messages.push(validatorField.checked.$message);
  }
  if (validatorField.anyChecked?.$invalid) {
    messages.push(validatorField.anyChecked.$message);
  }
  return messages.join("<br />");
};
