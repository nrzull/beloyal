import type { Mailing } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiMailingsGetAll = () =>
  apiClient
    .get<{ data: Mailing[] }>("/api/company/mailings")
    .then((v) => v.data);

export const apiMailingsCreate = (params: {
  text: string;
  shopIds: string[];
  withShops: boolean;
}) =>
  apiClient
    .post<{ data: Mailing }>("/api/company/mailings", params)
    .then((v) => v.data);
