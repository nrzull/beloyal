<template>
  <div
    v-if="links.length"
    ref="connectClientsRef"
    :class="$style.qrWrapper"
    v-html="connectClientsQr"
  />
</template>

<script setup lang="ts">
import { computed } from "vue";
import QrcodeGenerator from "qrcode-generator";
import type { CompanyLink } from "@beloyal/shared";

const { links } = defineProps<{ links: CompanyLink[] }>();

const connectClientsQr = computed(() => {
  const url = new URL(LINKS_WEB_PUBLIC_URL);

  url.searchParams.set(
    "payload",
    btoa(encodeURIComponent(JSON.stringify(links)))
  );

  const qr = QrcodeGenerator(0, "L");
  qr.addData(url.toString());
  qr.make();

  return qr.createSvgTag({ scalable: true });
});
</script>

<style module>
.qrWrapper svg {
  height: 500px;
  border: 4px solid rgba(0, 0, 0, 0.05);
  border-radius: 8px;
}
</style>
