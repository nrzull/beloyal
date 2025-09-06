import { apiClient } from "~/api/clients";

export const apiAuthLoginAsTelegramCustomer = (params: {
  query: string;
  tgBotId: string;
  hash: string;
}) =>
  apiClient
    .post<{ data: { token: string; expiredAt: string } }>(
      "/api/auth/miniapp/customer/telegram",
      params
    )
    .then((v) => v.data);
