<template>
  <div :class="$style.inputWrapper">
    <ChatModalInput
      v-if="withModal && model.attachments.length"
      :loading="loading"
      v-model="model"
      @close="model.attachments = []"
      @send="$emit('send')"
    />

    <Input
      ref="input"
      size="large"
      type="textarea"
      autosize
      placeholder="Введите сообщение"
      :loading="loading"
      :bordered="false"
      v-model:value="model.text"
    >
      <template #suffix>
        <Space align="center" :size="[16, 0]">
          <ChatAttachmentsInput
            :loading="loading"
            v-model="model.attachments"
          />

          <Button
            :type="canSend ? 'primary' : 'tertiary'"
            size="medium"
            :bordered="canSend"
            :disabled="!canSend || loading"
            @click="$emit('send')"
          >
            Отправить
          </Button>
        </Space>
      </template>
    </Input>
  </div>
</template>

<script setup lang="ts">
import { Space, Button, Input } from "@beloyal/shared";
import { computed, onMounted, useTemplateRef } from "vue";
import type { ChatInput as TChatInput } from "~/components/Chats/Messenger/types";
import ChatAttachmentsInput from "~/components/Chats/Messenger/ChatAttachmentsInput.vue";
import ChatModalInput from "~/components/Chats/Messenger/ChatModalInput.vue";

const { withModal = true } = defineProps<{
  loading: boolean;
  withModal?: boolean;
}>();

defineEmits<{ send: [] }>();

const inputRef = useTemplateRef("input");
const model = defineModel<TChatInput>({ required: true });

const canSend = computed(() =>
  Boolean(model.value.attachments.length || model.value.text.length)
);

onMounted(() => inputRef.value!.focus());
</script>

<style module>
.inputWrapper {
  background-color: #fff;
  padding: 12px;
  border-radius: 8px;
}
</style>
