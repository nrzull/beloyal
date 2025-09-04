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
    <div :class="$style.inputWrapper">
      <NInput
        ref="input"
        size="large"
        type="textarea"
        autosize
        placeholder="Введите сообщение"
        :loading="loading"
        :bordered="false"
        v-model:value="proxyInput"
      >
        <template #suffix>
          <NButton
            :type="input ? 'primary' : 'tertiary'"
            size="large"
            :bordered="!!input"
            :disabled="!input || loading"
            @click="$emit('send', chat)"
          >
            Отправить
          </NButton>
        </template>
      </NInput>
    </div>
  </div>
</template>

<script setup lang="ts">
import { NInput, NButton } from "naive-ui";
import type { Chat, ChatMessage as TChatMessage } from "~/api/chats/types";
import { computed, onMounted, useTemplateRef, watch } from "vue";
import ChatMessage from "~/components/Chats/Messenger/ChatMessage.vue";
import { self } from "~/composables/self";

self.value?.companyAccess?.id;

const messagesRef = useTemplateRef("messagesRef");

const { input, chat, messages } = defineProps<{
  chat: Chat;
  messages: TChatMessage[];
  loading: boolean;
  input?: any;
}>();

const emit = defineEmits<{
  "update:input": [{ chat: Chat; input: any }];
  send: [chat: Chat];
}>();

const proxyInput = computed({
  get: () => input,
  set: (v) => {
    emit("update:input", { chat, input: v });
  },
});

const inputRef = useTemplateRef("input");

onMounted(() => inputRef.value!.focus());

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

.inputWrapper {
  background-color: #fff;
  padding: 12px;
  border-radius: 8px;
}

.messages {
  display: flex;
  flex-flow: column-reverse;
  height: 100%;
  gap: 8px;
  overflow: auto;
}
</style>
