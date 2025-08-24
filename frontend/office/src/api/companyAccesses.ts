import { apiClient } from "~/api/clients";
import type { CompanyAccess } from "~/api/companyAccesses/types";

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
