import type { RouteLocationRaw } from "vue-router";

export type MenuRouteItem = {
  type: "route";
  route: RouteLocationRaw;
  label: string;
  available?: boolean;
  exact?: boolean;
};

export type MenuGroupItem = {
  type: "group";
  label: string;
  children: MenuRouteItem[];
  available?: boolean;
  exact?: boolean;
};

export type MenuItem = MenuGroupItem | MenuRouteItem;
