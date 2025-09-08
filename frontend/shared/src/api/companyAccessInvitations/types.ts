import type { Company } from "../companies/types";
import type { CompanyRole } from "../companyRoles/types";

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
