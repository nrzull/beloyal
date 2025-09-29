import type { Operation } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiOperationsGetAll = () =>
  apiClient
    .get<{ data: Operation[] }>("/api/agent/operations")
    .then((v) => v.data);
