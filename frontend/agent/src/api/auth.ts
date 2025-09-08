import { apiClient } from "~/api/clients";

export const apiAuth = (params: { email: string }) =>
  apiClient
    .post<{ expiredAt: string; code?: string }>("/api/auth", params)
    .then((v) => v.data);

export const apiAuthConfirm = (params: { email: string; code: string }) =>
  apiClient.post("/api/auth/confirm", params);

export const apiAuthLogout = () => apiClient.get("/api/auth/logout");
