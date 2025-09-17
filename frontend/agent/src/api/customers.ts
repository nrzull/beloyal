import { apiClient } from "~/api/clients";

export type Customer = Awaited<ReturnType<typeof apiCustomersGetByPublicToken>>;

export const apiCustomersGetByPublicToken = (params: { publicToken: string }) =>
  apiClient
    .get<{
      data: {
        bonusAmount: number;
        bonusMultiplier: number;
        phoneNumber: string;
        customerId: string;
      };
    }>(`/api/agent/customers/public-tokens/${params.publicToken}`)
    .then((v) => v.data);

export const apiCustomersAddBonus = (params: {
  publicToken: string;
  income: number;
}) =>
  apiClient
    .post(
      `/api/agent/customers/public-tokens/${params.publicToken}/add-bonus`,
      { income: params.income }
    )
    .then((v) => v.data);

export const apiCustomersSubtractBonus = (params: {
  publicToken: string;
  income: number;
}) =>
  apiClient
    .post(
      `/api/agent/customers/public-tokens/${params.publicToken}/subtract-bonus`,
      { income: params.income }
    )
    .then((v) => v.data);
