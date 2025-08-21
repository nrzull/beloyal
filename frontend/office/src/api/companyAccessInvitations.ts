import { apiClient } from "~/api/clients";
import type { CompanyAccessInvitation } from "~/api/companyAccessInvitations/types";

export const apiCompanyAccessInvitationsCreate = (params: {
  email: string;
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
