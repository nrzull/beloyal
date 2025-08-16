import type { Company } from "~/api/companies/types";
import type { CompanyRole } from "~/api/companyRoles/types";

export type CompanyAccessInvitationStatus =
  | "Active"
  | "Accepted"
  | "Rejected"
  | "Revoked";

export type CompanyAccessInvitation = {
  id: string;
  updatedAt: string;
  email: string;
  company: Company;
  role: CompanyRole;
  status: CompanyAccessInvitationStatus;
};
