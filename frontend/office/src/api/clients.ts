import axios from "axios";
import { HubConnectionBuilder } from "@microsoft/signalr";

export const apiClient = axios.create({
  baseURL: API_WEB_PUBLIC_URL,
  withCredentials: true,
});

export const wsClient = new HubConnectionBuilder()
  .withUrl(`${API_WEB_PUBLIC_URL}/api/ws`)
  .withAutomaticReconnect()
  .build();
