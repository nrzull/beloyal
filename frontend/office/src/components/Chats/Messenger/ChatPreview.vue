<template>
  <div :class="[$style.chatPreview, { [$style.active]: active }]">
    <div :class="$style.top">
      <div :class="$style.sender">{{ chat.name || chat.id }}</div>
      <div v-if="lastMessage" :class="$style.timestamp">
        {{ formatMessageTimestamp(lastMessage.createdAt) }}
      </div>
    </div>

    <div v-if="lastMessage" :class="$style.content">
      <span v-if="isYourLastMessage">Вы:</span>
      {{ lastMessage.content }}
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Chat, ChatMessage } from "@beloyal/shared";
import { computed } from "vue";
import { formatMessageTimestamp } from "@beloyal/shared";
import { self } from "~/composables/self";

const { messages, chat } = defineProps<{
  chat: Chat;
  active?: boolean;
  loading: boolean;
  messages: ChatMessage[];
}>();

const lastMessage = computed(() => messages[0] || chat.previewMessage);
const isYourLastMessage = computed(
  () => self.value?.companyAccess?.id === lastMessage?.value?.agentId
);
</script>

<style module>
.chatPreview {
  padding: 12px;
  background-color: rgba(255, 255, 255, 0.75);
  border-radius: 8px;
  display: grid;
  gap: 8px;
  cursor: pointer;
}

.active {
  background-color: var(--color-primary);
}

.active * {
  color: #fff !important;
}

.top {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  gap: 4px;
}

.sender {
  color: var(--color-primary);
  font-weight: 500;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  width: 120px;
}

.timestamp {
  opacity: 0.5;
  font-size: 12px;
  white-space: nowrap;
}

.content {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
</style>
