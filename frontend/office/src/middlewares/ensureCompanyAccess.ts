import { self } from "~/composables/self";

export const ensureCompanyAccess = (redirect: string) => () => {
  console.log(self.value);
  if (!self.value?.companyAccess) {
    return redirect;
  }
};
