import { createI18n } from "vue-i18n";
import { getKnownLocale, knownLocales } from "~/locales/utils";
import { ru_RU } from "~/locales/ru_RU";
import { en_US } from "~/locales/en_US";

export const globalI18n = createI18n({
  legacy: false,
  fallbackLocale: getKnownLocale(navigator.language),
  locale: getKnownLocale(navigator.language),
  messages: { [knownLocales.ru_RU]: ru_RU, [knownLocales.en_US]: en_US },
});
