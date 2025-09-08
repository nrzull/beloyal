import type {
  User,
  AdminAccess,
  CompanyAccess,
  CompanyAccessInvitation,
} from "@beloyal/shared";
import { apiClient } from "~/api/clients";

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
