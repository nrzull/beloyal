import type { Chat, ChatMessage } from "@beloyal/shared";
import { apiClient } from "~/api/clients";

export const apiChatsGetAll = () =>
  apiClient.get<{ data: Chat[] }>("/api/company/chats").then((v) => v.data);

export const apiChatsGet = (params: { chatId: string }) =>
  apiClient
    .get<{ data: Chat }>(`/api/company/chats/${params.chatId}`)
    .then((v) => v.data);

export const apiChatsGetAllMessages = (params: {
  chatId: string;
  withSystem: boolean;
}) =>
  apiClient
    .get<{ data: ChatMessage[] }>(
      `/api/company/chats/${params.chatId}/messages`,
      { params: { withSystem: params.withSystem } }
    )
    .then((v) => v.data);

export const apiChatsCreateMessage = (params: {
  chatId: string;
  content: string;
  attachments: File[];
}) => {
  const data = new FormData();
  data.set("content", params.content);

  for (const attachment of params.attachments) {
    data.append("attachments", attachment);
  }

  return apiClient
    .post(`/api/company/chats/${params.chatId}/messages`, data)
    .then((v) => v.data);
};
