<template>
  <Page>
    <template #top>
      <Heading>Новая торговая точка</Heading>
    </template>

    <Card class="v-space-12">
      <Space vertical>
        <div>Название</div>
        <Input
          :disabled="isLoading"
          placeholder="Название"
          v-model:value="name"
        />
      </Space>

      <Space vertical>
        <div>Временная зона</div>
        <NSelect
          placeholder="Временная зона"
          v-model:value="timeZone"
          clearable
          label-field="label"
          value-field="value"
          :options="timeZones"
          filterable
        />
      </Space>

      <Button :disabled="isLoading" type="primary" @click="createShop">
        Добавить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Space, Heading, Card, Button, Input } from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";
import { apiShopsCreate } from "~/api/shops";
import { NSelect } from "naive-ui";
import { timeZones } from "~/utils/tz";

const isLoading = computed(() => wait.some([createShop]));
const router = useRouter();
const name = ref<string>();
const timeZone = ref<string>();

const createShop = async () => {
  if (!name.value) {
    return;
  }

  try {
    wait.start(createShop);
    await apiShopsCreate({
      name: name.value,
      timeZone: timeZone.value || null,
    });
    router.push("/shops");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createShop);
  }
};
</script>
