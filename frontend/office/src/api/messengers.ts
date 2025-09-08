import type { TgBot } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiMessengersGetTgBot = () =>
  apiClient
    .get<{ data: TgBot }>("/api/company/messengers/telegram-bot")
    .then((v) => v.data);

export const apiMessengersCreateTgBot = (params: { token: string }) =>
  apiClient
    .post<{ data: TgBot }>("/api/company/messengers/telegram-bot", params)
    .then((v) => v.data);
