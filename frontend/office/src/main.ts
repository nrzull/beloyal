import "@beloyal/shared/main.css";
import { createApp } from "vue";
import { router } from "~/router";
import { globalI18n } from "~/locales";
import App from "~/App.vue";

const app = createApp(App).use(router).use(globalI18n);
router.isReady().then(() => app.mount("#app"));
