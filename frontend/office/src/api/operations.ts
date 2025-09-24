import { apiClient } from "~/api/clients";
import type { Operation } from "~/api/operations/types";

export const apiOperationsGetAll = () =>
  apiClient
    .get<{ data: Operation[] }>("/api/company/operations")
    .then((v) => v.data);
