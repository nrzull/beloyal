import { self } from "~/composables/self";

export const ensureCompanyAccess = (redirect: string) => () => {
  if (!self.value?.companyAccess && !self.value?.adminAccess) {
    return redirect;
  }
};
