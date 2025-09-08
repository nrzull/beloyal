<template>{{ bot.name }}</template>

<script setup lang="ts">
import type { TgBot } from "@beloyal/shared";

defineProps<{ bot: TgBot }>();
</script>
