import type { GlobalThemeOverrides } from "naive-ui";
import { computed } from "vue";

export const theme = computed(
  () =>
    ({
      common: {
        primaryColor: "#0073ff",
        primaryColorHover: "#1c82ff",
        primaryColorPressed: "#0073ff",
        primaryColorSuppl: "#0073ff",
      },
    } satisfies GlobalThemeOverrides)
);
