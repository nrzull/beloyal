import type { Booster, Customer, Operation } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiCustomersGetAll = () =>
  apiClient
    .get<{ data: Customer[] }>("/api/company/customers")
    .then((v) => v.data);

export const apiCustomersGet = (params: { customerId: string }) =>
  apiClient
    .get<{ data: Customer }>(`/api/company/customers/${params.customerId}`)
    .then((v) => v.data);

export const apiCustomersGetOperations = (params: { customerId: string }) =>
  apiClient
    .get<{ data: Operation[] }>(
      `/api/company/customers/${params.customerId}/operations`
    )
    .then((v) => v.data);

export const apiCustomersGetBoosters = (params: { customerId: string }) =>
  apiClient
    .get<{ data: Booster[] }>(
      `/api/company/customers/${params.customerId}/boosters`
    )
    .then((v) => v.data);

export const apiCustomersCreateBooster = (params: {
  customerId: string;
  cashbackPercent: number;
  expiredAt: string;
}) =>
  apiClient
    .post<{ data: Booster }>(
      `/api/company/customers/${params.customerId}/boosters`,
      params
    )
    .then((v) => v.data);
