<template>
  <NModal show :class="$style.modal" preset="card" @close="$emit('close')">
    <template #header>Редактирование торговой точки</template>

    <ShopForm
      :loading="loading"
      v-model="model"
      :country-time-zones="transformCountryTimeZones(countryZones)"
    />

    <template #footer>
      <Button
        :disabled="loading"
        type="primary"
        @click="$emit('submit', model)"
      >
        Сохранить
      </Button>
    </template>
  </NModal>
</template>

<script setup lang="ts">
import { NModal } from "naive-ui";
import { type Shop, Button, waitable } from "@beloyal/shared";
import { cloneDeep } from "es-toolkit";
import { ref } from "vue";
import ShopForm from "~/components/Shops/ShopForm.vue";
import { apiCountriesGetTimeZones } from "~/api/countries";
import { self } from "~/composables/self";
import { transformCountryTimeZones } from "~/components/Countries/utils";

const props = defineProps<{ shop: Shop; loading: boolean }>();

defineEmits<{ submit: [Shop]; close: [] }>();

const model = ref(cloneDeep(props.shop));
const countryZones = ref<string[]>([]);

waitable(apiCountriesGetTimeZones)({
  countryCode: self.value?.companyAccess!.company.countryCode!,
}).then((v) => (countryZones.value = v.data));
</script>

<style module>
.modal {
  max-width: 720px;
}
</style>
