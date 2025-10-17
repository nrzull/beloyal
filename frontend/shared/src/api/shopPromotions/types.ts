import type { Promotion } from "../promotions/types";

export type ShopPromotion = {
  id: string;
  promotion: Promotion;
  createdAt: string;
};
