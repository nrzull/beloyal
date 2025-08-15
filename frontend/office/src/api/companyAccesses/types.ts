import type { Company } from "~/api/companies/types";
import type { CompanyRole } from "~/api/companyRoles/types";

export type CompanyAccess = { id: string; company: Company; role: CompanyRole };
