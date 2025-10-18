export const timeZones = Intl.supportedValuesOf("timeZone").map((v) => ({
  value: v,
  label: v,
}));
