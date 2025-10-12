import type { Promotion, PromotionVariant } from "@beloyal/shared";

export type PartialPromotion = Pick<Promotion, "title"> & {
  variant: PromotionVariant | null;
};
