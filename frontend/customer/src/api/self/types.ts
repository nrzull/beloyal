import type { Level } from "@beloyal/shared";

export type Self = {
  companyName: string;
  companyLevels: Level[];
  bonusAmount: number;
  bonusMultiplier: number;
  levelPointsAmount: number;
};
