import { apiClient } from "~/api/clients";
import type { Shop } from "~/api/shops/types";

export const apiShopsGetAll = () =>
  apiClient.get<{ data: Shop[] }>("/api/company/shops").then((v) => v.data);

export const apiShopsGet = (params: { shopId: string }) =>
  apiClient
    .get<{ data: Shop }>(`/api/company/shops/${params.shopId}`)
    .then((v) => v.data);

export const apiShopsCreate = (params: { name: string }) =>
  apiClient
    .post<{ data: Shop }>("/api/company/shops", params)
    .then((v) => v.data);
