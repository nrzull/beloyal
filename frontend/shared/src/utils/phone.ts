import { maskitoTransform } from "@maskito/core";

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
  v ? maskitoTransform(v, { mask: phoneMask }) : v;
