import axios from "axios";
import { ref } from "vue";

export const authToken = ref<string>();

export const apiClient = axios.create({
  baseURL: API_WEB_PUBLIC_URL,
  withCredentials: true,
});

apiClient.interceptors.request.use((r) => {
  if (authToken.value) {
    r.headers.set("X-Customer-Token", `Bearer ${authToken.value}`);
  }

  return r;
});
