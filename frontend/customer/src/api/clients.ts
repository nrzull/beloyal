import axios from "axios";

export const apiClient = axios.create({
  baseURL: API_WEB_PUBLIC_URL,
  withCredentials: true,
});
