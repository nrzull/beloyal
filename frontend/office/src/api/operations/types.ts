import type { Bonus, CompanyAccess, Shop } from "@beloyal/shared";

export type Operation = {
  bonus: Bonus;
  shop?: Shop;
  agent?: CompanyAccess;
};
