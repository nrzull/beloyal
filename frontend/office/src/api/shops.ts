import type { Shop, ShopAccess } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

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

export const apiShopsGetAccesses = (params: { shopId: string }) =>
  apiClient
    .get<{ data: ShopAccess[] }>(
      `/api/company/shops/${params.shopId}/shop-accesses`
    )
    .then((v) => v.data);

export const apiShopsCreateAccess = (params: {
  shopId: string;
  companyAccessId: string;
}) =>
  apiClient
    .post<{ data: ShopAccess }>(
      `/api/company/shops/${params.shopId}/shop-accesses`,
      params
    )
    .then((v) => v.data);
