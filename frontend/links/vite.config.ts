import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

// @ts-ignore
const { LINKS_WEB_PORT, API_WEB_PUBLIC_URL } = process.env;

export default defineConfig({
  plugins: [vue()],

  base: "/links",

  resolve: {
    alias: {
      "~": new URL("./src", import.meta.url).pathname,
    },
  },

  server: {
    port: Number(LINKS_WEB_PORT),
    allowedHosts: [new URL(API_WEB_PUBLIC_URL).host],
  },

  define: {
    API_WEB_PUBLIC_URL: JSON.stringify(API_WEB_PUBLIC_URL),
  },
});
