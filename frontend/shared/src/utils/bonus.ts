import type { Bonus } from "../api";

const translations: Record<Bonus["type"], string> = {
  AddBonus: "Начисление",
  BurnBonus: "Сгорание",
  RevertBonus: "Возврат",
  SubtractBonus: "Списание",
};

export const translateBonusType = (v: Bonus["type"]) => translations[v] || v;
