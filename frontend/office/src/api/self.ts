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

export const apiSelfCreateCompany = (params: {
  tin: string;
  cashbackPercent: number;
  cashbackThreshold: number;
}) => apiClient.post("/api/self/company", params);
