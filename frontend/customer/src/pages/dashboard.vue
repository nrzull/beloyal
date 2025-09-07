<template>
  <div>{{ self?.bonusAmount }} {{ self?.companyName }}</div>
  <div :class="$style.qrWrapper" v-html="qrImage"></div>
</template>

<script setup lang="ts">
import { self } from "~/composables/self";
import QrcodeGenerator from "qrcode-generator";
import { publicToken } from "~/composables/publicToken";
import { computed } from "vue";

const qrImage = computed(() => {
  const qr = QrcodeGenerator(0, "L");
  qr.addData(publicToken.value!);
  qr.make();

  return qr.createSvgTag({ scalable: true });
});
</script>

<style module>
.qrWrapper {
}
</style>
