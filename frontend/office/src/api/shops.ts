import type { Shop, ShopAccess, ShopPromotion } from "@beloyal/shared";
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

export const apiShopsRemoveAccess = (params: {
  shopId: string;
  shopAccessId: string;
}) =>
  apiClient
    .delete<{ data: ShopAccess }>(
      `/api/company/shops/${params.shopId}/shop-accesses/${params.shopAccessId}`
    )
    .then((v) => v.data);

export const apiShopsGetPromotions = (params: { shopId: string }) =>
  apiClient
    .get<{ data: ShopPromotion[] }>(
      `/api/company/shops/${params.shopId}/shop-promotions`
    )
    .then((v) => v.data);

export const apiShopsAddPromotion = (params: {
  shopId: string;
  promotionId: string;
}) =>
  apiClient
    .post<{ data: ShopPromotion }>(
      `/api/company/shops/${params.shopId}/shop-promotions`,
      params
    )
    .then((v) => v.data);

export const apiShopsRemovePromotion = (params: {
  shopId: string;
  shopPromotionId: string;
}) =>
  apiClient
    .delete<{ data: ShopPromotion }>(
      `/api/company/shops/${params.shopId}/shop-promotions/${params.shopPromotionId}`
    )
    .then((v) => v.data);
