<template>
  <div :class="$style.chatConversation">
    <div ref="messagesRef" :class="$style.messages">
      <ChatMessage
        v-for="message in messages"
        :key="message.id"
        :message="message"
        :is-owner="message.agentId === self?.companyAccess?.id"
      />
    </div>

    <ChatInput
      :loading="loading"
      v-model="proxyInput"
      @send="$emit('send', chat)"
    />
  </div>
</template>

<script setup lang="ts">
import type { Chat, ChatMessage as TChatMessage } from "@beloyal/shared";
import { computed, useTemplateRef, watch } from "vue";
import ChatMessage from "~/components/Chats/Messenger/ChatMessage.vue";
import { self } from "~/composables/self";
import ChatInput from "~/components/Chats/Messenger/ChatInput.vue";
import type { ChatInput as TChatInput } from "~/components/Chats/Messenger/types";

const messagesRef = useTemplateRef("messagesRef");

const { input, chat, messages } = defineProps<{
  chat: Chat;
  messages: TChatMessage[];
  loading: boolean;
  input: TChatInput;
}>();

const emit = defineEmits<{
  "update:input": [{ chat: Chat; input: TChatInput }];
  send: [chat: Chat];
}>();

const proxyInput = computed({
  get: () => input,
  set: (v) => {
    emit("update:input", { chat, input: v });
  },
});

watch(
  () => messages,
  () => messagesRef?.value?.scrollTo({ top: 0 }),
  { deep: true, flush: "post" }
);
</script>

<style module>
.chatConversation {
  display: grid;
  grid-template-rows: calc(100vh - 200px) max-content;
  gap: 24px;
}

.messages {
  display: flex;
  flex-flow: column-reverse;
  height: 100%;
  gap: 8px;
  overflow: auto;
}
</style>
