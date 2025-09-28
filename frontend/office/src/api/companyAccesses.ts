import type { CompanyAccess } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiCompanyAccessesGetAll = () =>
  apiClient
    .get<{ data: CompanyAccess[] }>("/api/company/company-accesses")
    .then((v) => v.data);

export const apiCompanyAccessesGet = (params: { companyAccessId: string }) =>
  apiClient
    .get<{ data: CompanyAccess }>(
      `/api/company/company-accesses/${params.companyAccessId}`
    )
    .then((v) => v.data);

export const apiCompanyAccessesBlock = (params: { companyAccessId: string }) =>
  apiClient
    .patch<unknown>(
      `/api/company/company-accesses/${params.companyAccessId}/block`
    )
    .then((v) => v.data);

export const apiCompanyAccessesUnblock = (params: {
  companyAccessId: string;
}) =>
  apiClient
    .patch<unknown>(
      `/api/company/company-accesses/${params.companyAccessId}/unblock`
    )
    .then((v) => v.data);

export const apiCompanyAccessesCreate = (params: {
  phone: string;
  roleId: string;
}) =>
  apiClient
    .post<{ data: CompanyAccess }>(`/api/company/company-accesses`, params)
    .then((v) => v.data);

export const apiCompanyAccessesUpdate = (params: {
  companyAccessId: string;
  roleId: string;
}) =>
  apiClient
    .patch<{ data: CompanyAccess }>(
      `/api/company/company-accesses/${params.companyAccessId}`,
      params
    )
    .then((v) => v.data);
