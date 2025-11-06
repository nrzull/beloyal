import { createI18n } from "vue-i18n";
import { knownLocales } from "~/locales/utils";
import { ru_RU } from "~/locales/ru_RU";
import { en_US } from "~/locales/en_US";

export const globali18n = createI18n({
  legacy: false,
  fallbackLocale: knownLocales.ru_RU,
  locale: knownLocales.en_US,
  messages: { [knownLocales.ru_RU]: ru_RU, [knownLocales.en_US]: en_US },
});
