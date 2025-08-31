import { apiClient } from "~/api/clients";
import type { Mailing } from "~/api/mailings/types";

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
