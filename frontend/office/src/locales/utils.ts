export const knownLocales = {
  ru_RU: "ru_RU",
  en_US: "en_US",
} as const;

export type KnownLocale = keyof typeof knownLocales;

export const translateLocales: Record<KnownLocale, string> = {
  en_US: "English (United States)",
  ru_RU: "Русский",
};

export const getKnownLocale = (initialLocale: string) => {
  const key =
    Object.keys(knownLocales).find(
      (v) => transformLocaleKey(v) === transformLocaleKey(initialLocale)
    ) || knownLocales.ru_RU;

  return knownLocales[key as KnownLocale];
};

const transformLocaleKey = (v: string) =>
  v.replace(/[^a-zA-Z]+/g, "").toLowerCase();
