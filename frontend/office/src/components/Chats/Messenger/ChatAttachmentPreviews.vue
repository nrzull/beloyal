<template>
  <div :class="$style.previews">
    <template v-for="img in links">
      <div :class="$style.wrapper">
        <img
          v-if="!failedLinks.includes(img.link)"
          :src="img.link"
          :class="$style.preview"
          @click="tryOpen(img.link)"
          @error="failedLinks.push(img.link)"
        />
        <div v-else :class="[$style.preview, $style.failed]">
          <Icon variant="file" @click="tryOpen(img.link)" />
        </div>
        <div :class="$style.title">{{ img.title }}</div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { Icon } from "@beloyal/shared";

defineProps<{ links: Array<{ link: string; title: string }> }>();

const failedLinks = ref<string[]>([]);

const tryOpen = (v: string) => {
  const el = document.createElement("a");
  el.target = "_blank";

  if (v.startsWith("data:")) {
    const [type, base64] = v.split(",");
    const mimetype = type.split(":")[1].split(";")[0];

    if (!mimetype.startsWith("image")) {
      el.download = "file";
      el.href = v;
      el.click();
    } else {
      const bytes = atob(base64);
      const arr = new Uint8Array(new ArrayBuffer(bytes.length));
      for (let i = 0; i < bytes.length; i++) {
        arr[i] = bytes.charCodeAt(i);
      }

      const blob = new Blob([arr], { type: mimetype });
      const url = URL.createObjectURL(blob);
      el.href = url;
      el.click();

      URL.revokeObjectURL(url);
    }
  } else {
    el.href = v;
    el.click();
  }
};
</script>

<style module>
.previews {
  display: flex;
  gap: 4px;
  padding-bottom: 4px;
  overflow-x: auto;
}

.preview {
  height: 128px;
  max-width: 128px;
  object-fit: contain;
  border: 1px solid transparent;
  background-color: #fafafa;
  padding: 4px;
  user-select: none;
  -webkit-user-drag: none;
  cursor: pointer;
}

.wrapper {
  display: flex;
  flex-flow: column;
  gap: 8px;
  min-height: 128px;
}

.title {
  font-size: 12px;
}

.preview.failed {
  min-width: 128px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.preview:hover {
  border-color: var(--color-primary);
  color: var(--color-primary);
}
</style>
