import { apiClient } from "~/api/clients";

export const apiCountriesGetAll = () =>
  apiClient.get<{ data: string[] }>("/api/countries").then((v) => v.data);

export const apiCountriesGetLangs = (params: { countryCode: string }) =>
  apiClient
    .get<{ data: string[] }>(`/api/countries/${params.countryCode}/langs`)
    .then((v) => v.data);

export const apiCountriesGetTimeZones = (params: { countryCode: string }) =>
  apiClient
    .get<{ data: string[] }>(`/api/countries/${params.countryCode}/timeZones`)
    .then((v) => v.data);
