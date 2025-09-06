import { apiClient } from "~/api/clients";
import type { Self } from "~/api/self/types";

export const apiSelfGet = () =>
  apiClient.get<{ data: Self }>("/api/customer/self").then((v) => v.data);
