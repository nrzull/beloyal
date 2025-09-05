import "~/main.css";
import { createApp } from "vue";
import { router } from "~/router";
import App from "~/App.vue";
import { init, isTMA } from "@telegram-apps/sdk-vue";

if (isTMA()) {
  init();
}

const app = createApp(App).use(router);
router.isReady().then(() => app.mount("#app"));
