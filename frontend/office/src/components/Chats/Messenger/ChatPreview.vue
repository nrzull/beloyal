<template>
  <div
    v-if="chat.previewMessage"
    :class="[$style.chatPreview, { [$style.active]: active }]"
  >
    <div :class="$style.top">
      <div :class="$style.sender">{{ chat.previewMessage.name }}</div>
      <div :class="$style.timestamp">
        {{ formatMessageTimestamp(chat.previewMessage.createdAt) }}
      </div>
    </div>

    <div :class="$style.content">
      {{ chat.previewMessage.content }}
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Chat } from "~/api/chats/types";
import { formatMessageTimestamp } from "~/utils/date";

defineProps<{ chat: Chat; active?: boolean; loading: boolean }>();
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
