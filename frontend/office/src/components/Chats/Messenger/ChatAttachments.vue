<template>
  <ChatAttachmentPreviews :links="processedFiles.map((v) => v.preview)" />
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import ChatAttachmentPreviews from "~/components/Chats/Messenger/ChatAttachmentPreviews.vue";

defineProps<{ loading: boolean }>();

const model = defineModel<File[]>({ required: true });

const processedFiles = ref<Array<{ file: File; preview: string }>>([]);

const processFile = async (file: File): Promise<string> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(reader.result as string);
    reader.onerror = () => reject();
    reader.readAsDataURL(file);
  });

watch(
  () => model.value,
  async (v) => {
    const result = await Promise.allSettled(
      v.map(async (vv) => ({
        file: vv,
        preview: await processFile(vv),
      }))
    );

    processedFiles.value = result
      .filter((v) => v.status === "fulfilled")
      .map((v) => v.value);
  },
  { deep: true, immediate: true }
);
</script>
