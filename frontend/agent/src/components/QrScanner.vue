<template>
  <div id="qr-code-full-region"></div>
</template>

<script setup lang="ts">
import { Html5QrcodeScanner, Html5QrcodeScanType } from "html5-qrcode";

const emit = defineEmits<{
  scan: [data: string];
}>();

const scanQr = () => {
  const scanner = new Html5QrcodeScanner(
    "qr-code-full-region",
    { fps: 10, supportedScanTypes: [Html5QrcodeScanType.SCAN_TYPE_CAMERA] },
    undefined
  );

  scanner.render((token) => {
    scanner.clear();
    emit("scan", token);
  }, undefined);
};

defineExpose({ scanQr });
</script>
