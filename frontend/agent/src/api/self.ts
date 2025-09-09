import type { User, CompanyAccess, Shop } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiSelfGet = () =>
  apiClient
    .get<{
      user: User;
      shop?: Shop;
      shops: Array<Shop>;
      companyAccess?: CompanyAccess;
      companyAccesses: CompanyAccess[];
    }>("/api/agent/self")
    .then((v) => v.data);

export const apiSelfSetCompanyAccess = (params: { companyAccessId: string }) =>
  apiClient.patch("/api/agent/self/company-access", params).then((v) => v.data);

export const apiSelfSetShopAccess = (params: {
  companyAccessId: string;
  shopId: string;
}) =>
  apiClient.patch("/api/agent/self/shop-access", params).then((v) => v.data);
