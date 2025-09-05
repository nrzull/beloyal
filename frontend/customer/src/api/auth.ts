import { apiClient } from "~/api/clients";

export const apiAuthLoginAsTelegramCustomer = (params: {
  query: string;
  tgBotId: string;
  hash: string;
}) => apiClient.post("/api/auth/miniapp/customer/telegram", params);
