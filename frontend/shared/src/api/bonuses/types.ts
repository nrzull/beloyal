import type { PromotionVariant } from "../promotions/types";

export type Bonus = {
  id: string;
  createdAt: string;
  type: "AddBonus" | "SubtractBonus" | "RevertBonus" | "BurnBonus";
  amount: number;
  cashIncome: number;
  rawCashIncome: number;
  cashbackPercent: number;
  promotionType?: PromotionVariant["$type"];
  scheduledAt?: string;
  status: "Done" | "Scheduled" | "Cancelled";
};
