<template>
  <Page>
    <template #top>
      <Heading>Кэшбэк</Heading>
    </template>

    <Card>
      <Space vertical :size="[0, 16]">
        <Space vertical>
          <div>Процент кэшбэка</div>
          <Input size="large" placeholder="Укажите процент">
            <template #prefix>
              <Icon
                :style="{ color: 'var(--color-grey-500)' }"
                variant="percent"
              />
            </template>
          </Input>
        </Space>

        <Space vertical>
          <div>Максимальное покрытие суммы кэшбэком</div>
          <Input size="large" placeholder="Укажите процент">
            <template #prefix>
              <Icon
                :style="{ color: 'var(--color-grey-500)' }"
                variant="percent"
              />
            </template>
          </Input>
        </Space>

        <Space vertical>
          <div>Визуальный мультипликатор бонусов</div>
          <Input size="large" placeholder="Укажите число" />
        </Space>

        <Space vertical>
          <div>Период сгорания бонусов (в днях)</div>
          <Input
            size="large"
            placeholder="Укажите число, через сколько дней должны сгореть бонусы"
          />
        </Space>

        <Button type="primary" block size="large">Сохранить</Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading } from "@beloyal/shared";
import { Card, Button, Input, Space, Icon } from "@beloyal/shared";
</script>
