<template>
  <NSpace v-if="self" vertical :size="[0, 24]">
    <NAlert type="info" title="Бонусы">
      <template #icon>
        <Icon variant="coins" />
      </template>

      У вас <b>{{ self?.bonusAmount }}</b> Бонусов
    </NAlert>

    <div :class="$style.qrWrapper" v-html="qrImage"></div>
  </NSpace>
</template>

<script setup lang="ts">
import { NSpace, NAlert } from "naive-ui";
import { self } from "~/composables/self";
import QrcodeGenerator from "qrcode-generator";
import { publicToken } from "~/composables/publicToken";
import { computed } from "vue";
import { Icon } from "@beloyal/shared";

const qrImage = computed(() => {
  const qr = QrcodeGenerator(0, "L");
  qr.addData(publicToken.value!);
  qr.make();

  return qr.createSvgTag({ scalable: true });
});
</script>

<style module>
.qrWrapper {
  border: 4px solid rgba(0, 0, 0, 0.05);
  border-radius: 8px;
}
</style>
