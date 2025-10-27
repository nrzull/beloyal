import type { Booster } from "@beloyal/shared";
import { formatISO, startOfDay } from "date-fns";

export const newBooster = (): Pick<
  Booster,
  "cashbackPercent" | "expiredAt"
> => ({ cashbackPercent: 1, expiredAt: formatISO(startOfDay(new Date())) });
