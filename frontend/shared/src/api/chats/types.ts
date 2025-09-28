export type Chat = { id: string; name: string; previewMessage?: ChatMessage };

export type ChatMessage = {
  id: string;
  name: string;
  content?: string;
  agentId?: string;
  customerId?: string;
  readAt?: string;
  createdAt: string;
  chatId: string;
  mailingUnitId?: string;
};
