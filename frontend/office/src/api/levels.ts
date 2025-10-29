import { apiClient } from "~/api/clients";
import type { Level } from "@beloyal/shared";

export const apiLevelsGetAll = () =>
  apiClient.get<{ data: Level[] }>("/api/company/levels").then((v) => v.data);

export const apiLevelsGet = (params: { id: string }) =>
  apiClient
    .get<{ data: Level }>(`/api/company/levels/${params.id}`)
    .then((v) => v.data);

export const apiLevelsCreate = (params: {
  cashbackPercent: number;
  pointsCapacity: number;
  description?: string;
}) =>
  apiClient
    .post<{ data: Level }>("/api/company/levels", params)
    .then((v) => v.data);

export const apiLevelsUpdate = (params: {
  id: string;
  cashbackPercent: number;
  pointsCapacity: number;
  description?: string;
}) =>
  apiClient
    .patch<{ data: Level }>(`/api/company/levels/${params.id}`, params)
    .then((v) => v.data);

export const apiLevelsDelete = (params: { id: string }) =>
  apiClient
    .delete<unknown>(`/api/company/levels/${params.id}`)
    .then((v) => v.data);
