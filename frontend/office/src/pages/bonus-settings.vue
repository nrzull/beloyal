<template>
  <Page>
    <template #top>
      <Heading>Бонусы</Heading>
    </template>

    <Card v-if="bonusOptions">
      <Space vertical :size="[0, 16]">
        <Space vertical>
          <div>Процент кэшбэка</div>
          <CashbackPercentInput v-model="bonusOptions.cashbackPercent" />
        </Space>

        <Space vertical>
          <div>Максимальное покрытие суммы кэшбэком</div>
          <NInputNumber
            size="large"
            :min="1"
            :max="100"
            placeholder="Укажите процент"
            :precision="0"
            v-model:value="bonusOptions.cashbackThreshold"
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
            v-model:value="bonusOptions.bonusMultiplier"
          />
        </Space>

        <Space vertical>
          <div>Период сгорания бонусов (в днях)</div>
          <NInputNumber
            size="large"
            :min="1"
            :precision="0"
            placeholder="Укажите число, через сколько дней должны сгореть бонусы"
            v-model:value="bonusOptions.bonusLifetimeInDays"
          />
        </Space>

        <Card with-border>
          <NThing>
            <template #header>
              <Heading>Отсрочка начислений</Heading>
            </template>

            <template #header-extra>
              <Button
                v-if="!bonusOptions.bonusScheduling"
                type="primary"
                ghost
                size="small"
                @click="
                  bonusOptions.bonusScheduling = serialize({
                    days: 0,
                    hours: 1,
                    minutes: 0,
                  })
                "
              >
                Добавить
              </Button>
              <Button
                v-else
                type="error"
                ghost
                size="small"
                @click="bonusOptions.bonusScheduling = null"
              >
                Удалить
              </Button>
            </template>

            <DurationForm
              v-if="bonusOptions.bonusScheduling"
              v-model="bonusOptions.bonusScheduling"
            />
          </NThing>
        </Card>

        <Card with-border>
          <NThing>
            <template #header>
              <Space vertical :size="[0, 2]">
                <Heading>Лимит операций (антифрод)</Heading>
                <Subheading type="notice"
                  >Пример: клиент сможет совершить
                  {{ bonusOptions.bonusFrequencyCount }} операций в течение
                  {{ bonusOptions.bonusFrequencyInDays }} дней</Subheading
                >
              </Space>
            </template>

            <Space vertical :size="[0, 16]">
              <Space vertical>
                <div>Период (в днях)</div>
                <NInputNumber
                  size="large"
                  :min="1"
                  :precision="0"
                  placeholder="Укажите число, в течение скольких дней можно совершить N операций"
                  v-model:value="bonusOptions.bonusFrequencyInDays"
                />
              </Space>

              <Space vertical>
                <div>Кол-во операций, доступных за период</div>
                <NInputNumber
                  size="large"
                  :min="1"
                  :precision="0"
                  placeholder="Укажите число"
                  v-model:value="bonusOptions.bonusFrequencyCount"
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
          @click="setBonusOptions"
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
import { Card, Button, Space, Icon, Subheading } from "@beloyal/shared";
import { computed, ref } from "vue";
import {
  apiCompanyGetBonusOptions,
  apiCompanySetBonusOptions,
} from "~/api/company";
import { NInputNumber, NThing } from "naive-ui";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";
import DurationForm from "~/components/DurationForm.vue";
import { serialize } from "tinyduration";

const bonusOptions = ref<CompanyBonusOptions>();

const isLoading = computed(() => wait.some([setBonusOptions]));

const setBonusOptions = async () => {
  try {
    wait.start(setBonusOptions);
    await apiCompanySetBonusOptions(bonusOptions.value!);
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(setBonusOptions);
  }
};

apiCompanyGetBonusOptions().then((v) => (bonusOptions.value = v.data));
</script>
