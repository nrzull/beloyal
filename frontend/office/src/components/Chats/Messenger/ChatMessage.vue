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
import type { ChatMessage } from "@beloyal/shared";
import { formatDate } from "@beloyal/shared";

defineProps<{ message: ChatMessage; isOwner: boolean }>();
</script>

<style module>
.chatMessage {
  background-color: #fff;
  width: max-content;
  min-width: 200px;
  max-width: 375px;
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

.content {
  line-height: 1.3;
  white-space: pre-line;
  overflow-wrap: break-word;
}

.bottom {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  gap: 4px;
}
</style>
