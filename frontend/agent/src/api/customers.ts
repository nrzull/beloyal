import { apiClient } from "~/api/clients";

export const apiCustomersGetByPublicToken = (params: { publicToken: string }) =>
  apiClient
    .get<{
      data: { bonusAmount: number; phoneNumber: string; customerId: string };
    }>(`/api/agent/customers/public-tokens/${params.publicToken}`)
    .then((v) => v.data);
