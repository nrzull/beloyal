import { ref } from "vue";
import { apiSelfGet } from "~/api/self";

export const self = ref<Awaited<ReturnType<typeof apiSelfGet>>>();

export const fetchSelf = async () => {
  try {
    self.value = await apiSelfGet();
  } catch (e) {
    console.error(e);
  }
};
