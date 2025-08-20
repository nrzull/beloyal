import { apiClient } from "~/api/clients";
import type { CompanyRole } from "~/api/companyRoles/types";

export const apiCompanyRolesGetAll = () =>
  apiClient
    .get<{ data: CompanyRole[] }>("/api/company/company-roles")
    .then((v) => v.data);
