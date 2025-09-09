import { self, fetchSelf } from "~/composables/self";

export const ensureSelf = (redirect: string) => async () => {
  await fetchSelf();

  if (!self.value) {
    return redirect;
  }
};
