import { computed, ref } from "vue";
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

export const bonusAmount = computed(() => {
  if (!self.value) {
    return 0;
  }

  const baseAmount = self.value.bonusAmount < 0 ? 0 : self.value.bonusAmount;

  return baseAmount * self.value.bonusMultiplier;
});
