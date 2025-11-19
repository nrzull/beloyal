import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

const { CUSTOMER_WEB_PORT, CUSTOMER_WEB_PUBLIC_URL, API_WEB_PUBLIC_URL } =
  // @ts-ignore
  process.env;

export default defineConfig({
  plugins: [vue()],

  base: "/customer",

  resolve: {
    alias: {
      "~": new URL("./src", import.meta.url).pathname,
    },
  },

  server: {
    port: Number(CUSTOMER_WEB_PORT),
    allowedHosts: CUSTOMER_WEB_PUBLIC_URL
      ? [new URL(CUSTOMER_WEB_PUBLIC_URL).host]
      : [],
  },

  define: {
    API_WEB_PUBLIC_URL: JSON.stringify(API_WEB_PUBLIC_URL),
  },
});
