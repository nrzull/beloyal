import { apiClient } from "~/api/clients";
import type { Mailing } from "~/api/mailings/types";

export const apiMailingsGetAll = () =>
  apiClient
    .get<{ data: Mailing[] }>("/api/company/mailings")
    .then((v) => v.data);
