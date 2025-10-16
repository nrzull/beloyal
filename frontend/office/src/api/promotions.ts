import type { Promotion, PromotionVariant } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiPromotionsGetAll = () =>
  apiClient
    .get<{ data: Promotion[] }>("/api/company/promotions")
    .then((v) => v.data);

export const apiPromotionsGet = (params: { id: string }) =>
  apiClient
    .get<{ data: Promotion }>(`/api/company/promotions/${params.id}`)
    .then((v) => v.data);

export const apiPromotionsCreate = (params: {
  title: string;
  variant: PromotionVariant;
}) =>
  apiClient
    .post<{ data: Promotion }>("/api/company/promotions", params)
    .then((v) => v.data);

export const apiPromotionsUpdate = (params: Promotion) =>
  apiClient
    .patch<{ data: Promotion }>(`/api/company/promotions/${params.id}`, params)
    .then((v) => v.data);
