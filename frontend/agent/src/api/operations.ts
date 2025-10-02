import type { Operation } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiOperationsGetAll = () =>
  apiClient
    .get<{ data: Operation[] }>("/api/agent/operations")
    .then((v) => v.data);

export const apiOperationsCancel = (params: { bonusId: string }) =>
  apiClient.patch(`/api/agent/operations/${params.bonusId}/cancel`);
