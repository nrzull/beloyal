import type { CompanyRole } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiCompanyRolesGetAll = () =>
  apiClient
    .get<{ data: CompanyRole[] }>("/api/company/company-roles")
    .then((v) => v.data);

export const apiCompanyRolesGet = (params: { roleId: string }) =>
  apiClient
    .get<{ data: CompanyRole }>(`/api/company/company-roles/${params.roleId}`)
    .then((v) => v.data);

export const apiCompanyRolesCreate = (params: {
  name: string;
  permissions: string[];
}) =>
  apiClient
    .post<{ data: CompanyRole }>("/api/company/company-roles", params)
    .then((v) => v.data);

export const apiCompanyRolesUpdate = (params: {
  id: string;
  name: string;
  permissions: string[];
}) =>
  apiClient
    .patch<{ data: CompanyRole }>(
      `/api/company/company-roles/${params.id}`,
      params
    )
    .then((v) => v.data);

export const apiCompanyRolesGetPermissions = () =>
  apiClient
    .get<{ data: string[] }>("/api/company/company-roles/permissions")
    .then((v) => v.data);
