<template>
  <NModal show :class="$style.modal" preset="card" @close="$emit('close')">
    <template #header>Редактирование торговой точки</template>

    <ShopForm :loading="loading" v-model="model" />

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
import { type Shop, Button } from "@beloyal/shared";
import { cloneDeep } from "es-toolkit";
import { ref } from "vue";
import ShopForm from "~/components/Shops/ShopForm.vue";

const props = defineProps<{ shop: Shop; loading: boolean }>();

defineEmits<{ submit: [Shop]; close: [] }>();

const model = ref(cloneDeep(props.shop));
</script>

<style module>
.modal {
  max-width: 720px;
}
</style>
