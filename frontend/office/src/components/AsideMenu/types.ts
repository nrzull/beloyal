import type { RouteLocationRaw } from "vue-router";

export type TAsideMenuItem = {
  link: RouteLocationRaw;
  title: string;
  available: boolean;
  exact?: true;
};
