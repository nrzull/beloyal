export const adminRolePermissions = [
  "white_companies_read",
  "white_companies_write",
] as const;

export type AdminRolePermission = (typeof adminRolePermissions)[number];

export type AdminRole = {
  id: string;
  name: string;
  createdAt: string;
  permissions: AdminRolePermission[];
};
