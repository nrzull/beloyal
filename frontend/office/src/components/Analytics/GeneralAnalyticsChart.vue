<template>
  <VChart :class="$style.chart" :option="option" />
</template>

<script setup lang="ts">
import { use } from "echarts/core";
import { SVGRenderer } from "echarts/renderers";
import { LineChart } from "echarts/charts";
import {
  TitleComponent,
  TooltipComponent,
  LegendComponent,
  GridComponent,
} from "echarts/components";
import VChart, { THEME_KEY } from "vue-echarts";
import { provide, computed } from "vue";
import type { apiAnalyticsGetGeneral } from "~/api/analytics";

const { value } = defineProps<{
  value: Awaited<ReturnType<typeof apiAnalyticsGetGeneral>>;
}>();

use([
  LineChart,
  TitleComponent,
  TooltipComponent,
  LegendComponent,
  GridComponent,
  SVGRenderer,
]);

provide(THEME_KEY, "light");

const option = computed(() => ({
  xAxis: {
    data: value.data.cashIncomeValues.map((v) => v.date),
  },
  yAxis: {},
  series: [
    {
      data: value.data.cashIncomeValues.map((v) => v.cashIncome),
      type: "line",
      smooth: true,
      label: {
        show: true,
        position: "bottom",
      },
    },
  ],
}));
</script>

<style module>
.chart {
  height: 256px;
}
</style>
