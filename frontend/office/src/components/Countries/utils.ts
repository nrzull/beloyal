export const transformCountryCodes = (v: string[]) =>
  v.map((vv) => ({ code: vv, title: vv }));

export const transformCountryTimeZones = (v: string[]) =>
  v.map((vv) => ({ timeZone: vv, title: vv }));
