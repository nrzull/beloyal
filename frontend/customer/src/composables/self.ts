import { ref } from "vue";
import type { Self } from "~/api/self/types";
import { apiSelfGet } from "~/api/self";

export const self = ref<Self>();
export const fetchSelf = async () => {
  try {
    const result = await apiSelfGet();
    self.value = result.data;
  } catch (e) {
    console.error(e);
  }
};
