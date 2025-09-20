export type Bonus = {
  id: string;
  createdAt: string;
  type: "AddBonus" | "SubtractBonus" | "RevertBonus" | "BurnBonus";
  amount: number;
  cashIncome: number;
};
