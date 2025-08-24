import type { AdminAccess } from "~/api/adminAccesses/types";
import { apiClient } from "~/api/clients";
import type { CompanyAccess } from "~/api/companyAccesses/types";
import type { CompanyAccessInvitation } from "~/api/companyAccessInvitations/types";
import type { User } from "~/api/users/types";

export const apiSelfGet = () =>
  apiClient
    .get<{
      user: User;
      adminAccess?: AdminAccess;
      companyAccess?: CompanyAccess;
      companyAccesses: CompanyAccess[];
    }>("/api/self")
    .then((v) => v.data);

export const apiSelfGetCompanyAccessInvitations = () =>
  apiClient
    .get<{ data: CompanyAccessInvitation[] }>(
      "/api/self/company-access-invitations"
    )
    .then((v) => v.data);

export const apiSelfAcceptCompanyAccessInvitation = (params: {
  companyAccessInvitationId: string;
}) =>
  apiClient.post(
    `/api/self/company-access-invitations/${params.companyAccessInvitationId}/accept`
  );

export const apiSelfCreateCompany = (params: {
  tin: string;
  cashbackPercent: number;
  cashbackThreshold: number;
}) => apiClient.post("/api/self/company", params);
