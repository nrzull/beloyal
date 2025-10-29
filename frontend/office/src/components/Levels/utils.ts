import type { Level } from "@beloyal/shared";

export const newLevel = (): Pick<
  Level,
  "cashbackPercent" | "description" | "pointsCapacity"
> => ({ cashbackPercent: 1, pointsCapacity: 1, description: "" });
