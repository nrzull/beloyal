<template>
  {{ shop?.name }}
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRoute } from "vue-router";
import { apiShopsGet } from "~/api/shops";
import type { Shop } from "~/api/shops/types";

const route = useRoute();
const shopId = computed(() => route.params.id as string);

const shop = ref<Shop>();

apiShopsGet({ shopId: shopId.value }).then((v) => (shop.value = v.data));
</script>
