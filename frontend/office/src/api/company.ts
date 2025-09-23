import type { CompanyCashback, CompanyLink } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiCompanyGetCashback = () =>
  apiClient
    .get<{ data: CompanyCashback }>("/api/company/cashback")
    .then((v) => v.data);

export const apiCompanySetCashback = (params: CompanyCashback) =>
  apiClient
    .patch<{ data: CompanyCashback }>("/api/company/cashback", params)
    .then((v) => v.data);

export const apiCompanyGetLinks = () =>
  apiClient
    .get<{ data: CompanyLink[] }>("/api/company/links")
    .then((v) => v.data);
