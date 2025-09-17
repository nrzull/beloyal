import type { ComputedRef } from "vue";
import { useMask } from "./useMask";

export const usePhoneMask = (el: ComputedRef<HTMLInputElement>) => {
  useMask(el, [
    "+",
    "7",
    " ",
    "(",
    /\d/,
    /\d/,
    /\d/,
    ")",
    " ",
    /\d/,
    /\d/,
    /\d/,
    "-",
    /\d/,
    /\d/,
    /\d/,
    /\d/,
  ]);
};
