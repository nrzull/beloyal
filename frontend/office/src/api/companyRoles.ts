import { apiClient } from "~/api/clients";
import type { CompanyRole } from "~/api/companyRoles/types";

export const apiCompanyRolesGetAll = () =>
  apiClient
    .get<{ data: CompanyRole[] }>("/api/company/company-roles")
    .then((v) => v.data);

export const apiCompanyRolesCreate = (params: {
  name: string;
  permissions: string[];
}) =>
  apiClient
    .post<{ data: CompanyRole }>("/api/company/company-roles", params)
    .then((v) => v.data);

export const apiCompanyRolesGetPermissions = () =>
  apiClient
    .get<{ data: string[] }>("/api/company/company-roles/permissions")
    .then((v) => v.data);
