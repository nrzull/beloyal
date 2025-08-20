import type { Company } from "~/api/companies/types";
import type { CompanyRole } from "~/api/companyRoles/types";
import type { User } from "~/api/users/types";

export type CompanyAccess = {
  id: string;
  company: Company;
  role: CompanyRole;
  user: User;
};
