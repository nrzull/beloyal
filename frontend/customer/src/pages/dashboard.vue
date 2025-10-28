<template>
  <Space v-if="self" vertical :size="[0, 24]">
    <Space vertical :size="[0, 8]">
      <NAlert type="info" v-if="level">
        Уровень: <b>{{ level.levelNumber }}</b>
        <br />
        Очки: {{ level.currentPoints }} / {{ level.pointsCapacity }}
      </NAlert>

      <NAlert type="info">
        <template #icon>
          <Icon variant="coins" />
        </template>

        У вас <b>{{ bonusAmount }}</b> Бонусов
      </NAlert>
    </Space>

    <div :class="$style.qrWrapper" v-html="qrImage"></div>
  </Space>
</template>

<script setup lang="ts">
import { NAlert } from "naive-ui";
import { self, bonusAmount, level } from "~/composables/self";
import QrcodeGenerator from "qrcode-generator";
import { publicToken } from "~/composables/publicToken";
import { computed } from "vue";
import { Space, Icon } from "@beloyal/shared";

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
