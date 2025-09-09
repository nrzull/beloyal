import type { CompanyAccessInvitation } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiCompanyAccessInvitationsCreate = (params: {
  phone: string;
  roleId: string;
}) =>
  apiClient
    .post<{ data: CompanyAccessInvitation }>(
      "/api/company/company-access-invitations",
      params
    )
    .then((v) => v.data);

export const apiCompanyAccessInvitationsGetAll = () =>
  apiClient
    .get<{ data: CompanyAccessInvitation[] }>(
      "/api/company/company-access-invitations"
    )
    .then((v) => v.data);
