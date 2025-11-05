import type { IconVariant } from "@beloyal/shared";
import type { RouteLocationRaw } from "vue-router";

export type MenuRouteItem = {
  type: "route";
  route: RouteLocationRaw;
  label: string;
  available?: boolean;
  exact?: boolean;
  icon?: IconVariant;
};

export type MenuGroupItem = {
  type: "group";
  label: string;
  children: MenuRouteItem[];
  available?: boolean;
  exact?: boolean;
  icon?: IconVariant;
};

export type MenuItem = MenuGroupItem | MenuRouteItem;
