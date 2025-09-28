export type Bonus = {
  id: string;
  createdAt: string;
  type: "AddBonus" | "SubtractBonus" | "RevertBonus" | "BurnBonus";
  amount: number;
  cashIncome: number;
  scheduledAt?: string;
  status: "Done" | "Scheduled" | "Cancelled";
};
