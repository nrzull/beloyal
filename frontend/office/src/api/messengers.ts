import { apiClient } from "~/api/clients";
import type { TgBot } from "~/api/messengers/types";

export const apiMessengersGetTgBot = () =>
  apiClient
    .get<{ data: TgBot }>("/api/company/messengers/telegram-bot")
    .then((v) => v.data);

export const apiMessengersCreateTgBot = (params: { token: string }) =>
  apiClient
    .post<{ data: TgBot }>("/api/company/messengers/telegram-bot", params)
    .then((v) => v.data);
