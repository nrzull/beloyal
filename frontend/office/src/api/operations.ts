import { apiClient } from "~/api/clients";
import type { Operation } from "~/api/operations/types";

export const apiOperationsGetAll = () =>
  apiClient
    .get<{ data: Operation[] }>("/api/company/operations")
    .then((v) => v.data);

export const apiOperationsCancel = (params: { bonusId: string }) =>
  apiClient
    .patch<unknown>(`/api/company/operations/${params.bonusId}/cancel`)
    .then((v) => v.data);
