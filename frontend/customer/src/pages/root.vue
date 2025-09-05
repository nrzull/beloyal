<template>
  {{ $route.params.messenger }}

  {{ parseInitDataQuery(retrieveRawInitData()!) }}
</template>

<script setup lang="ts">
import {
  retrieveRawInitData,
  parseInitDataQuery,
} from "@telegram-apps/sdk-vue";
</script>
