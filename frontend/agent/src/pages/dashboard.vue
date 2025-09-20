<template>
  <Space vertical>
    <Button size="large" type="primary" block @click="qrScannerRef?.scanQr()">
      <template #icon> <Icon variant="qr" /> </template>
      Сканировать
    </Button>

    <QrScanner
      ref="qrScannerRef"
      @scan="$router.push(`/customers/${$event}`)"
    />
  </Space>
</template>

<script setup lang="ts">
import { Icon, Button, Space } from "@beloyal/shared";
import QrScanner from "~/components/QrScanner.vue";
import { useTemplateRef } from "vue";

const qrScannerRef = useTemplateRef("qrScannerRef");
</script>
