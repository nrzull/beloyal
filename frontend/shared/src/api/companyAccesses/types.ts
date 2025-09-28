import type { Company } from "../companies/types";
import type { CompanyRole } from "../companyRoles/types";
import type { User } from "../users/types";

export type CompanyAccess = {
  id: string;
  company: Company;
  role: CompanyRole;
  user: User;
  blockedAt?: string;
};
