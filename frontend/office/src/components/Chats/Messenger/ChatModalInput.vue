<template>
  <NModal show :class="$style.modal" preset="card" @close="$emit('close')">
    <ChatAttachments :loading v-model="model.attachments" />
    <ChatInput
      :loading="loading"
      :with-modal="false"
      v-model="model"
      @send="$emit('send')"
    />
  </NModal>
</template>

<script setup lang="ts">
import { NModal } from "naive-ui";
import { defineAsyncComponent } from "vue";
import type { ChatInput as TChatInput } from "~/components/Chats/Messenger/types";
import ChatAttachments from "~/components/Chats/Messenger/ChatAttachments.vue";

defineProps<{ loading: boolean }>();
defineEmits<{ send: []; close: [] }>();

const model = defineModel<TChatInput>({ required: true });
const ChatInput = defineAsyncComponent(() => import("./ChatInput.vue"));
</script>

<style module>
.modal {
  max-width: 720px;
}
</style>
