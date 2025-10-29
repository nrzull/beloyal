<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>QR-Наклейки</Heading>
    </template>

    <Card>
      <Space vertical>
        <div>Подключение клиентов</div>
        <ConnectClientsQr :links="connectClientLinks" />
        <Button
          :style="{ width: '500px' }"
          size="large"
          type="primary"
          block
          @click="printConnectClients"
        >
          Распечатать
        </Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import { computed, createSSRApp, ref, type Component } from "vue";
import Page from "~/components/Page.vue";
import {
  Heading,
  Card,
  Space,
  type CompanyLink,
  wait,
  waitable,
} from "@beloyal/shared";
import { Button } from "@beloyal/shared";
import { renderToString } from "vue/server-renderer";
import ConnectClientsQr from "~/components/QrStickers/ConnectClientsQr.vue";
import { apiCompanyGetLinks } from "~/api/company";

const connectClientLinks = ref<CompanyLink[]>([]);

const isLoading = computed(() =>
  wait.some([apiCompanyGetLinks, printConnectClients])
);

const print = async (component: Component) => {
  const app = createSSRApp(component, { links: connectClientLinks.value });
  const html = await renderToString(app);

  const iframe = document.createElement("iframe");
  iframe.style.opacity = "0";
  document.body.appendChild(iframe);
  iframe.contentDocument!.body.innerHTML = html;
  iframe.focus();
  iframe.contentWindow?.print();
  iframe.parentElement?.removeChild(iframe);
};

const printConnectClients = async () => {
  await print(ConnectClientsQr);
};

waitable(apiCompanyGetLinks)().then((v) => (connectClientLinks.value = v.data));
</script>
