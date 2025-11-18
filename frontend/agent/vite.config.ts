import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

// @ts-ignore
const { AGENT_WEB_PORT, API_WEB_PUBLIC_URL } = process.env;

export default defineConfig({
  plugins: [vue()],

  base: "/agent",

  resolve: {
    alias: {
      "~": new URL("./src", import.meta.url).pathname,
    },
  },

  server: {
    port: Number(AGENT_WEB_PORT),
  },

  define: {
    API_WEB_PUBLIC_URL: JSON.stringify(API_WEB_PUBLIC_URL),
  },
});
