<template>
  <NSpace vertical>
    <NButton size="large" type="primary" block @click="scanQr">
      <template #icon> <Icon variant="qr" /> </template>
      Сканировать
    </NButton>

    <NButton tertiary size="large" type="primary" block>
      <template #icon> <Icon variant="search" /> </template>
      Поиск по номеру телефона
    </NButton>

    <div id="qr-code-full-region"></div>
  </NSpace>
</template>

<script setup lang="ts">
import { Icon } from "@beloyal/shared";
import { NButton, NSpace } from "naive-ui";
import { Html5QrcodeScanner, Html5QrcodeScanType } from "html5-qrcode";

const scanQr = () => {
  const scanner = new Html5QrcodeScanner(
    "qr-code-full-region",
    { fps: 10, supportedScanTypes: [Html5QrcodeScanType.SCAN_TYPE_CAMERA] },
    undefined
  );

  scanner.render((_result) => {
    scanner.clear();
  }, undefined);
};
</script>
