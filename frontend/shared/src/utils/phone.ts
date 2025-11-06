import { maskitoTransform } from "@maskito/core";
import { maskitoPhoneOptionsGenerator } from "@maskito/phone";
import metadata from "libphonenumber-js/min/metadata";

export const phoneMask = [
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
];

export const unmaskPhone = (v: string = "") =>
  v ? `+${v.replace(/[^\d]/g, "")}` : v;

export const maskPhone = (v: string = "") =>
  v
    ? maskitoTransform(
        v,
        maskitoPhoneOptionsGenerator({ metadata, strict: false })
      )
    : v;
