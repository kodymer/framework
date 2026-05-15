import { createApp } from "vue";
import { createPinia } from "pinia";
import PrimeVue from "primevue/config";

import App from "./App.vue";
import router from "@/router";
import i18n from "@/config/i18n";
import es from "@/config/i18n/locales/es.json";

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(PrimeVue, {
  locale: es.primevue,
});
app.use(i18n);

app.mount("#app");
