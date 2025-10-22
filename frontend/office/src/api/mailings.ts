import type { Mailing } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiMailingsGetAll = () =>
  apiClient
    .get<{ data: Mailing[] }>("/api/company/mailings")
    .then((v) => v.data);

export const apiMailingsCreate = (params: {
  content: string;
  shopIds: string[];
  withShops: boolean;
  attachments: File[];
}) => {
  const data = new FormData();
  data.set("content", params.content);
  data.set("withShops", String(params.withShops));

  for (const shopId of params.shopIds) {
    data.append("shopIds", shopId);
  }

  for (const attachment of params.attachments) {
    data.append("attachments", attachment);
  }

  apiClient
    .post<{ data: Mailing }>("/api/company/mailings", data)
    .then((v) => v.data);
};
