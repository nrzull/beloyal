import { apiClient } from "~/api/clients";
import type { WhiteCompany } from "~/api/whiteCompanies/types";

export const apiWhiteCompaniesGetAll = () =>
  apiClient
    .get<{ data: WhiteCompany[] }>("/api/white-companies")
    .then((v) => v.data);

export const apiWhiteCompaniesCreate = (params: {
  tin: string;
  expiredAt: string;
}) =>
  apiClient
    .post<{ data: WhiteCompany }>("/api/white-companies", params)
    .then((v) => v.data);
