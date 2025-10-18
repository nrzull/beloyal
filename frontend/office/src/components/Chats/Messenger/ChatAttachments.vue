<template>
  <div :class="$style.previews">
    <img
      v-for="img in processedFiles"
      :key="img.preview"
      :src="img.preview"
      :class="$style.preview"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";

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
    console.log(v);

    processedFiles.value = await Promise.all(
      v.map(async (vv) => ({
        file: vv,
        preview: await processFile(vv),
      }))
    );
  },
  { deep: true, immediate: true }
);
</script>

<style module>
.previews {
  overflow-x: auto;
  display: flex;
  gap: 4px;
  padding-bottom: 4px;
}

.preview {
  max-height: 128px;
  max-width: 128px;
  object-fit: contain;
  border: 1px solid transparent;
  background-color: #fafafa;
  padding: 4px;
  user-select: none;
  -webkit-user-drag: none;
}
</style>
