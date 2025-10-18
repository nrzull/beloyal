import type { Shop } from "@beloyal/shared";

export type PartialShop = Pick<Shop, "name" | "timeZone">;
