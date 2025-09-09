import { self } from "~/composables/self";

export const ensureShop = (redirect: string) => () => {
  if (
    !self.value?.companyAccess ||
    !self.value.shop ||
    self.value.shop.companyId !== self.value.companyAccess.company?.id
  ) {
    return redirect;
  }
};
