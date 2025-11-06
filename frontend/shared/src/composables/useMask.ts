import {
  Maskito,
  maskitoInitialCalibrationPlugin,
  type MaskitoMask,
} from "@maskito/core";
import { watch, ref, type ComputedRef, onUnmounted } from "vue";
import { maskitoPhoneOptionsGenerator } from "@maskito/phone";
import metadata from "libphonenumber-js/min/metadata";

export const useMask = (
  el: ComputedRef<HTMLInputElement>,
  mask: MaskitoMask
) => {
  const maskito = ref<Maskito>();
  const unsub = watch(
    el,
    (v) => {
      if (v) {
        maskito.value?.destroy();
        maskito.value = new Maskito(v, {
          mask,
          plugins: [maskitoInitialCalibrationPlugin()],
          overwriteMode: "shift",
        });
      }
    },
    { deep: true, immediate: true }
  );

  onUnmounted(() => {
    unsub();
    maskito.value?.destroy();
  });
};

export const usePhoneMask = (el: ComputedRef<HTMLInputElement>) => {
  const maskito = ref<Maskito>();
  const unsub = watch(
    el,
    (v) => {
      if (v) {
        maskito.value?.destroy();
        maskito.value = new Maskito(
          v,
          maskitoPhoneOptionsGenerator({ metadata })
        );
      }
    },
    { deep: true, immediate: true }
  );

  onUnmounted(() => {
    unsub();
    maskito.value?.destroy();
  });
};
