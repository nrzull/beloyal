import type { Chat, ChatMessage } from "~/api/chats/types";
import { apiClient } from "~/api/clients";

export const apiChatsGetAll = () =>
  apiClient.get<{ data: Chat[] }>("/api/company/chats").then((v) => v.data);

export const apiChatsGetAllMessages = (params: { chatId: string }) =>
  apiClient
    .get<{ data: ChatMessage[] }>(
      `/api/company/chats/${params.chatId}/messages`
    )
    .then((v) => v.data);
