<template>{{ bot.name }}</template>

<script setup lang="ts">
import type { TgBot } from "~/api/messengers/types";

defineProps<{ bot: TgBot }>();
</script>
