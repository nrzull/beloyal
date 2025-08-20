import { apiClient } from "~/api/clients";
import type { CompanyAccess } from "~/api/companyAccesses/types";

export const apiCompanyAccessesGetAll = () =>
  apiClient
    .get<{ data: CompanyAccess[] }>("/api/company/company-accesses")
    .then((v) => v.data);
