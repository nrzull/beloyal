import { apiClient } from "~/api/clients";

export const apiAnalyticsGetGeneral = (params: {
  filters: { from: string; to: string; shopId?: string };
}) =>
  apiClient
    .get<{
      data: {
        cashIncome: number;
        rawCashIncome: number;
        averageCheck: number;
        operationsCount: number;
        customersCount: number;
        cashIncomeValues: Array<{
          date: string;
          cashIncome: number;
          rawCashIncome: number;
        }>;
      };
    }>("/api/company/analytics/general", {
      params: {
        filters: JSON.stringify(params.filters),
      },
    })
    .then((v) => v.data);
