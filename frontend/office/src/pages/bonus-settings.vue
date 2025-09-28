<template>
  <Page>
    <template #top>
      <Heading>Бонусы</Heading>
    </template>

    <Card v-if="companyCashback">
      <Space vertical :size="[0, 16]">
        <Space vertical>
          <div>Процент кэшбэка</div>
          <NInputNumber
            size="large"
            placeholder="Укажите процент"
            :min="1"
            :max="100"
            :precision="0"
            v-model:value="companyCashback.cashbackPercent"
          >
            <template #prefix>
              <Icon
                :style="{ color: 'var(--color-grey-500)' }"
                variant="percent"
              />
            </template>
          </NInputNumber>
        </Space>

        <Space vertical>
          <div>Максимальное покрытие суммы кэшбэком</div>
          <NInputNumber
            size="large"
            :min="1"
            :max="100"
            placeholder="Укажите процент"
            :precision="0"
            v-model:value="companyCashback.cashbackThreshold"
          >
            <template #prefix>
              <Icon
                :style="{ color: 'var(--color-grey-500)' }"
                variant="percent"
              />
            </template>
          </NInputNumber>
        </Space>

        <Space vertical>
          <div>Визуальный мультипликатор бонусов</div>
          <NInputNumber
            size="large"
            :min="1"
            :max="100"
            :precision="0"
            placeholder="Укажите процент"
            v-model:value="companyCashback.bonusMultiplier"
          />
        </Space>

        <Space vertical>
          <div>Период сгорания бонусов (в днях)</div>
          <NInputNumber
            size="large"
            :min="1"
            :precision="0"
            placeholder="Укажите число, через сколько дней должны сгореть бонусы"
            v-model:value="companyCashback.bonusLifetimeInDays"
          />
        </Space>

        <Card with-border>
          <NThing>
            <template #header>
              <Heading>Лимит операций (антифрод)</Heading>
            </template>

            <Space vertical :size="[0, 16]">
              <Space vertical>
                <div>Период (в днях)</div>
                <NInputNumber
                  size="large"
                  :min="1"
                  :precision="0"
                  placeholder="Укажите число, в течение скольких дней можно совершить N операций"
                  v-model:value="companyCashback.bonusFrequencyInDays"
                />
              </Space>

              <Space vertical>
                <div>Кол-во операций, доступных за период</div>
                <NInputNumber
                  size="large"
                  :min="1"
                  :precision="0"
                  placeholder="Укажите число"
                  v-model:value="companyCashback.bonusFrequencyCount"
                />
              </Space>
            </Space>
          </NThing>
        </Card>

        <Button
          type="primary"
          block
          size="large"
          :disabled="isLoading"
          @click="setCashback"
        >
          Сохранить
        </Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, wait, type CompanyBonusOptions } from "@beloyal/shared";
import { Card, Button, Space, Icon } from "@beloyal/shared";
import { computed, ref } from "vue";
import {
  apiCompanyGetBonusOptions,
  apiCompanySetBonusOptions,
} from "~/api/company";
import { NInputNumber, NThing } from "naive-ui";

const companyCashback = ref<CompanyBonusOptions>();

const isLoading = computed(() => wait.some([setCashback]));

const setCashback = async () => {
  try {
    wait.start(setCashback);
    await apiCompanySetBonusOptions(companyCashback.value!);
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(setCashback);
  }
};

apiCompanyGetBonusOptions().then((v) => (companyCashback.value = v.data));
</script>
