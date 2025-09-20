import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";
import axios from "axios";
import { ref } from "vue";

export const authToken = ref<string>();

export const apiClient = axios.create({
  baseURL: API_WEB_PUBLIC_URL,
  withCredentials: true,
});

export const wsClient = new HubConnectionBuilder()
  .withUrl(`${API_WEB_PUBLIC_URL}/api/ws`, {
    accessTokenFactory: () => authToken.value!,
    transport: HttpTransportType.LongPolling,
  })
  .withAutomaticReconnect()
  .build();

apiClient.interceptors.request.use((r) => {
  if (authToken.value) {
    r.headers.set("X-Customer-Token", `Bearer ${authToken.value}`);
  }

  return r;
});
