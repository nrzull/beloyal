import { self } from "~/composables/self";

export const ensureNoCompanyAccess = (redirect: string) => () => {
  if (self.value?.companyAccess || self.value?.adminAccess) {
    return redirect;
  }
};
