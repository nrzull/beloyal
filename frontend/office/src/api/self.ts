import type { User, AdminAccess, CompanyAccess } from "@beloyal/shared";
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

export const apiSelfUpdate = (params: {
  firstName?: string;
  lastName?: string;
  middleName?: string;
}) => apiClient.patch<unknown>("/api/self", params).then((v) => v.data);

export const apiSelfCreateCompany = (params: {
  tin: string;
  title: string;
  cashbackPercent: number;
  cashbackThreshold: number;
  bonusMultiplier: number;
  bonusLifetimeInDays: number;
}) => apiClient.post("/api/self/company", params);

export const apiSelfSetCompanyAccess = (params: {
  companyAccessId: string | null;
}) => apiClient.patch("/api/self/company-access", params).then((v) => v.data);
