import type { Bonus } from "../api";

const typeTranslations: Record<Bonus["type"], string> = {
  AddBonus: "Начисление",
  BurnBonus: "Сгорание",
  RevertBonus: "Возврат",
  SubtractBonus: "Списание",
};

const statusTranslations: Record<Bonus["status"], string> = {
  Cancelled: "Отменено",
  Done: "Завершено",
  Scheduled: "Отложено",
};

export const translateBonusType = (v: Bonus["type"]) =>
  typeTranslations[v] || v;

export const translateBonusStatus = (v: Bonus["status"]) =>
  statusTranslations[v] || v;
