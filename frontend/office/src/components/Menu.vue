<template>
  <div :class="$style.menu">
    <template v-for="item in items">
      <MenuRouteButton v-if="item.type === 'route'" :item />

      <template v-else-if="item.type === 'group'">
        <MenuButton
          :label="item.label"
          :active="hasActiveChild(item)"
          @click="expanded.set(item, true)"
        />

        <template v-if="expanded.get(item)">
          <MenuRouteButton
            v-for="child in item.children"
            :key="child.label"
            :item="child"
            :class="$style.child"
          />
        </template>
      </template>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import MenuButton from "~/components/Menu/MenuButton.vue";
import MenuRouteButton from "~/components/Menu/MenuRouteButton.vue";
import type { MenuGroupItem, MenuItem } from "~/components/Menu/types";

const { items } = defineProps<{ items: MenuItem[] }>();

const router = useRouter();
const route = useRoute();
const expanded = ref<Map<MenuGroupItem, boolean>>(new Map());

const hasActiveChild = (v: MenuGroupItem) =>
  v.children.some((child) => {
    const resolved = router.resolve(child.route);
    return resolved && route.path.startsWith(resolved.path);
  });

const syncWithRoutes = () =>
  items.forEach((item) => {
    if (item.type === "group" && hasActiveChild(item)) {
      expanded.value.set(item, true);
    }
  });

watch(() => route.path, syncWithRoutes, { flush: "post", immediate: true });
</script>

<style module>
.menu {
  padding: 12px 24px;
  padding-left: 36px;
  height: auto;
  line-height: 1;
}

.child {
  padding-left: 12px;
  border-left: 1px solid rgba(0, 0, 0, 0.1);
}

.arrow {
  margin-left: auto;
}

.arrow:not(.active) {
  transform: rotate(-90deg);
}
</style>
