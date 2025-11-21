<template>
  <Space v-if="self" vertical :size="[0, 24]">
    <Space vertical :size="[0, 8]">
      <Card size="s" v-if="level">
        Уровень: <b>{{ level.levelNumber }}</b>
        <br />
        Очки: {{ level.currentPoints }} / {{ level.pointsCapacity }}
      </Card>

      <Card size="s">
        <Space align="center">
          <Icon :style="{ color: 'var(--color-primary)' }" variant="coins" />
          <span
            >У вас <b>{{ bonusAmount }}</b> Бонусов</span
          >
        </Space>
      </Card>
    </Space>

    <div :class="$style.qrWrapper" v-html="qrImage"></div>
  </Space>
</template>

<script setup lang="ts">
import { self, bonusAmount, level } from "~/composables/self";
import QrcodeGenerator from "qrcode-generator";
import { publicToken } from "~/composables/publicToken";
import { computed } from "vue";
import { Space, Icon } from "@beloyal/shared";
import { Card } from "@beloyal/shared";

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
