import type { Operation } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiOperationsGetAll = () =>
  apiClient
    .get<{ data: Operation[] }>("/api/company/operations")
    .then((v) => v.data);

export const apiOperationsCancel = (params: { bonusId: string }) =>
  apiClient
    .patch<unknown>(`/api/company/operations/${params.bonusId}/cancel`)
    .then((v) => v.data);
