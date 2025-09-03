<template>
  <Card :with-shadow="false" :class="$style.chatConversation">
    <div :class="$style.messages"></div>
    <NInput
      ref="input"
      size="large"
      type="textarea"
      autosize
      placeholder="Введите сообщение"
      :loading="loading"
      v-model:value="proxyInput"
    >
      <template #suffix>
        <NButton
          :type="input ? 'primary' : 'tertiary'"
          size="small"
          :disabled="!input || loading"
        >
          Отправить
        </NButton>
      </template>
    </NInput>
  </Card>
</template>

<script setup lang="ts">
import Card from "~/components/Card/Card.vue";
import { NInput, NButton } from "naive-ui";
import type { Chat, ChatMessage } from "~/api/chats/types";
import { computed, onMounted, useTemplateRef } from "vue";

const { input, chat } = defineProps<{
  chat: Chat;
  messages: ChatMessage[];
  loading: boolean;
  input?: any;
}>();

const emit = defineEmits<{ "update:input": [{ chat: Chat; input: any }] }>();

const proxyInput = computed({
  get: () => input,
  set: (v) => {
    emit("update:input", { chat, input: v });
  },
});

const inputRef = useTemplateRef("input");

onMounted(() => inputRef.value!.focus());
</script>

<style module>
.chatConversation {
  display: flex;
  flex-flow: column;
  justify-content: flex-end;
}
</style>
