import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

const {
  OFFICE_WEB_PORT,
  OFFICE_WEB_PUBLIC_URL,
  API_WEB_PUBLIC_URL,
  LINKS_WEB_PUBLIC_URL,
} =
  // @ts-ignore
  process.env;

export default defineConfig({
  plugins: [vue()],

  base: "/office",

  resolve: {
    alias: {
      "~": new URL("./src", import.meta.url).pathname,
    },
  },

  server: {
    port: Number(OFFICE_WEB_PORT),
    allowedHosts: [new URL(OFFICE_WEB_PUBLIC_URL).host],
  },

  define: {
    API_WEB_PUBLIC_URL: JSON.stringify(API_WEB_PUBLIC_URL),
    LINKS_WEB_PUBLIC_URL: JSON.stringify(LINKS_WEB_PUBLIC_URL),
  },
});
