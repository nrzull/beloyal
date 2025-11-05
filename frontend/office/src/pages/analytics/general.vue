<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Аналитика</Heading>
    </template>

    <Card class="v-space-24">
      <Card with-border :with-shadow="false" :class="$style.filters">
        <Space vertical>
          <div>Торговая точка</div>
          <NSelect
            :options="shops"
            placeholder="Торговая точка"
            value-field="id"
            label-field="name"
            clearable
            v-model:value="filters.shopId"
            @update:value="fetchAnalytics"
          />
        </Space>

        <Space vertical>
          <div>Дата от</div>
          <NDatePicker
            placeholder="От"
            v-model:value="proxyFrom"
            @update:value="fetchAnalytics"
          />
        </Space>

        <Space vertical>
          <div>Дата до</div>
          <NDatePicker
            bloc
            placeholder="До"
            v-model:value="proxyTo"
            @update:value="fetchAnalytics"
          />
        </Space>
      </Card>

      <Card
        v-if="analytics"
        with-border
        :with-shadow="false"
        :class="$style.analytics"
      >
        <Card :class="$style.cashIncome" with-border :with-shadow="false">
          <Page>
            <template #top> Прибыль: {{ analytics.data.cashIncome }}</template>

            <GeneralAnalyticsChart v-if="analytics" :value="analytics" />
          </Page>
        </Card>

        <Card>
          <Page>
            <template #top>Доход</template>
            {{ analytics.data.rawCashIncome }}
          </Page>
        </Card>

        <Card>
          <Page>
            <template #top>Средний чек</template>
            {{ analytics.data.averageCheck }}
          </Page>
        </Card>

        <Card>
          <Page>
            <template #top>Кол-во клиентов</template>
            {{ analytics.data.customersCount }}
          </Page>
        </Card>

        <Card>
          <Page>
            <template #top>Кол-во операций</template>
            {{ analytics.data.operationsCount }}
          </Page>
        </Card>
      </Card>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import { apiAnalyticsGetGeneral } from "~/api/analytics";
import { addMonths, formatISO, getUnixTime } from "date-fns";
import Page from "~/components/Page.vue";
import { Heading, wait, waitable, type Shop } from "@beloyal/shared";
import { computed, ref } from "vue";
import { useToast } from "@beloyal/shared";
import { apiShopsGetAll } from "~/api/shops";
import { NSelect, NDatePicker } from "naive-ui";
import { Space, Card } from "@beloyal/shared";
import GeneralAnalyticsChart from "~/components/Analytics/GeneralAnalyticsChart.vue";

const isLoading = computed(() => wait.some([apiShopsGetAll, fetchAnalytics]));

const { showError } = useToast();

const filters = ref<{ from: string; to: string; shopId?: string }>({
  from: formatISO(addMonths(new Date(), -1)),
  to: formatISO(new Date()),
  shopId: undefined,
});

const proxyFrom = computed({
  get: () =>
    filters.value.from ? getUnixTime(filters.value.from) * 1000 : null,
  set: (v) => (filters.value.from = formatISO(v!)),
});

const proxyTo = computed({
  get: () => (filters.value.to ? getUnixTime(filters.value.to) * 1000 : null),
  set: (v) => (filters.value.to = formatISO(v!)),
});

const analytics = ref<Awaited<ReturnType<typeof apiAnalyticsGetGeneral>>>();

const shops = ref<Shop[]>([]);

const fetchAnalytics = async () => {
  try {
    wait.start(fetchAnalytics);
    analytics.value = await apiAnalyticsGetGeneral({ filters: filters.value });
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(fetchAnalytics);
  }
};

waitable(apiShopsGetAll)().then((v) => (shops.value = v.data));

fetchAnalytics();
</script>

<style module>
.filters {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 16px;
}

.analytics {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 16px;
}

.cashIncome {
  grid-column: 1/3;
}
</style>
