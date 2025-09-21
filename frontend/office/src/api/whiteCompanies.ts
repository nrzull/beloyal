import type { WhiteCompany } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiWhiteCompaniesGetAll = () =>
  apiClient
    .get<{ data: WhiteCompany[] }>("/api/white-companies")
    .then((v) => v.data);

export const apiWhiteCompaniesGet = (params: { id: string }) =>
  apiClient
    .get<{ data: WhiteCompany }>(`/api/white-companies/${params.id}`)
    .then((v) => v.data);

export const apiWhiteCompaniesCreate = (params: {
  tin: string;
  expiredAt: string;
}) =>
  apiClient
    .post<{ data: WhiteCompany }>("/api/white-companies", params)
    .then((v) => v.data);

export const apiWhiteCompaniesUpdate = (params: {
  id: string;
  expiredAt: string;
}) =>
  apiClient
    .patch<{ data: WhiteCompany }>(`/api/white-companies/${params.id}`, params)
    .then((v) => v.data);
