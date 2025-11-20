import { maskitoTransform } from "@maskito/core";
import { maskitoPhoneOptionsGenerator } from "@maskito/phone";
import metadata from "libphonenumber-js/min/metadata";
import { parsePhoneNumberFromString } from "libphonenumber-js";

export const unmaskPhone = (v: string = "") =>
  v ? `+${v.replace(/[^\d]/g, "")}` : v;

export const maskPhone = (v: string = "") =>
  v
    ? maskitoTransform(
        v,
        maskitoPhoneOptionsGenerator({
          metadata,
          countryIsoCode: parsePhoneNumberFromString(v)?.country,
          strict: true,
        })
      )
    : v;
