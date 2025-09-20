<template>
  <div :class="$style.messenger">
    <div :class="$style.previews">
      <ChatPreview
        v-for="chat in chats.keys()"
        :key="chat.id"
        :chat="chat"
        :messages="chats.get(chat) || []"
        :loading="isLoading"
        :active="activeChat === chat"
        @click="activeChat = chat"
      />
    </div>

    <ChatConversation
      v-if="activeChat"
      :key="activeChat?.id"
      :loading="isLoading"
      :chat="activeChat"
      :messages="chats.get(activeChat) || []"
      :input="inputHistory.get(activeChat)"
      @update:input="inputHistory.set($event.chat, $event.input)"
      @send="sendMessage"
    />
  </div>
</template>

<script setup lang="ts">
import { computed, onUnmounted, ref } from "vue";
import {
  apiChatsGetAll,
  apiChatsGetAllMessages,
  apiChatsCreateMessage,
} from "~/api/chats";
import type { Chat, ChatMessage } from "@beloyal/shared";
import { wait } from "@beloyal/shared";
import ChatPreview from "~/components/Chats/Messenger/ChatPreview.vue";
import ChatConversation from "~/components/Chats/Messenger/ChatConversation.vue";
import { wsClient } from "~/api/clients";

const isLoading = computed(() => wait.some([fetchChats]));
const chats = ref<Map<Chat, ChatMessage[]>>(new Map());
const activeChat = ref<Chat>();
const inputHistory = ref<Map<Chat, string>>(new Map());

const fetchChats = async () => {
  try {
    wait.start(fetchChats);

    for (const chat of (await apiChatsGetAll()).data) {
      const { data: messages } = await apiChatsGetAllMessages({
        chatId: chat.id,
      });

      chats.value.set(chat, messages);
    }
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchChats);
  }
};

const sendMessage = async (chat: Chat) => {
  const input = inputHistory.value.get(chat);
  if (!input) {
    return;
  }

  try {
    wait.start(sendMessage);

    inputHistory.value.set(chat, "");
    await apiChatsCreateMessage({ chatId: chat.id, content: input });
  } catch (e) {
    console.error(e);

    if (!inputHistory.value.has(chat)) {
      inputHistory.value.set(chat, input);
    }
  } finally {
    wait.end(sendMessage);
  }
};

const onChatMessage = (message: ChatMessage) => {
  const chat = Array.from(chats.value.keys()).find(
    (v) => v.id === message.chatId
  );

  if (!chat) {
    console.error("TODO: fetch chat with preview message");
  } else {
    const messages = chats.value.get(chat);
    messages?.unshift(message);
  }
};

wsClient.on("ChatMessage", onChatMessage);
onUnmounted(() => wsClient.off("ChatMessage", onChatMessage));

fetchChats();
</script>

<style module>
.messenger {
  height: inherit;
  display: grid;
  grid-template-columns: 240px 1fr;
  overflow: hidden;
  gap: 8px;
}

.previews {
  background-color: #fafafa;
  display: flex;
  flex-flow: column;
  gap: 4px;
}
</style>
