import type { CompanyBonusOptions, CompanyLink } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiCompanyGetBonusOptions = () =>
  apiClient
    .get<{ data: CompanyBonusOptions }>("/api/company/cashback")
    .then((v) => v.data);

export const apiCompanySetBonusOptions = (params: CompanyBonusOptions) =>
  apiClient
    .patch<{ data: CompanyBonusOptions }>("/api/company/cashback", params)
    .then((v) => v.data);

export const apiCompanyGetLinks = () =>
  apiClient
    .get<{ data: CompanyLink[] }>("/api/company/links")
    .then((v) => v.data);
