<template>
  <div :class="$style.menu">
    <template v-for="item in items">
      <MenuRouteButton v-if="item.type === 'route'" :item />

      <template v-else-if="item.type === 'group'">
        <MenuButton
          :label="item.label"
          :active="hasActiveChild(item)"
          @click="expanded.set(item, !expanded.get(item))"
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
import { ref } from "vue";
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

syncWithRoutes();
</script>

<style module>
.menu {
  padding: 12px 0;
  background-color: rgba(0, 0, 0, 0.025);
  height: auto;
  line-height: 1;
}

.child {
  margin-left: 12px;
}

.arrow {
  margin-left: auto;
}

.arrow:not(.active) {
  transform: rotate(-90deg);
}
</style>
