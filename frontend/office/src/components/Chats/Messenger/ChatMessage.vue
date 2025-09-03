<template>
  <div :class="[$style.chatMessage, { [$style.owner]: isOwner }]">
    <div v-if="!isOwner" :class="$style.sender">{{ message.name }}</div>
    <div :class="$style.content">{{ message.content }}</div>
    <div :class="$style.bottom">
      <div></div>
      <div :class="$style.timestamp">
        {{ formatDate(message.createdAt) }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { ChatMessage } from "~/api/chats/types";
import { formatDate } from "~/utils/date";

defineProps<{ message: ChatMessage; isOwner: boolean }>();
</script>

<style module>
.chatMessage {
  background-color: #fff;
  width: max-content;
  min-width: 25%;
  max-width: 50%;
  padding: 12px 18px;
  border-radius: 16px 4px;
  flex-flow: column;
  display: flex;
  gap: 8px;
}

.chatMessage.owner {
  background-color: var(--color-primary-light);
}

.sender {
  font-weight: 500;
}

.timestamp {
  opacity: 0.5;
  font-size: 12px;
  white-space: nowrap;
}

.bottom {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  gap: 4px;
}
</style>
