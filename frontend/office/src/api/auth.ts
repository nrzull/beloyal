import { apiClient } from "~/api/clients";

export const apiAuth = (params: { phone: string }) =>
  apiClient
    .post<{ expiredAt: string; code?: string }>("/api/auth", params)
    .then((v) => v.data);

export const apiAuthConfirm = (params: { phone: string; code: string }) =>
  apiClient.post("/api/auth/confirm", params);

export const apiAuthLogout = () => apiClient.get("/api/auth/logout");
