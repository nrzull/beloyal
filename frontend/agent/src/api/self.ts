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
