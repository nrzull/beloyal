<template>
  <NSpace vertical>
    <NButton size="large" type="primary" block @click="qrScannerRef?.scanQr()">
      <template #icon> <Icon variant="qr" /> </template>
      Сканировать
    </NButton>

    <QrScanner
      ref="qrScannerRef"
      @scan="$router.push(`/customers/${$event}`)"
    />
  </NSpace>
</template>

<script setup lang="ts">
import { Icon } from "@beloyal/shared";
import { NButton, NSpace } from "naive-ui";
import QrScanner from "~/components/QrScanner.vue";
import { useTemplateRef } from "vue";

const qrScannerRef = useTemplateRef("qrScannerRef");
</script>
