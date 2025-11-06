export const knownLocales = {
  ru_RU: "ru-RU",
  en_US: "en-US",
  en_CA: "en-US",
} as const;

export const getKnownLocale = (initialLocale: string) => {
  const key =
    Object.keys(knownLocales).find(
      (v) => transformLocaleKey(v) === transformLocaleKey(initialLocale)
    ) || "ru_RU";

  return knownLocales[key as keyof typeof knownLocales];
};

const transformLocaleKey = (v: string) =>
  v.replace(/[^a-zA-Z]+/g, "").toLowerCase();
