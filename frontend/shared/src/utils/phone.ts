export const unmaskPhone = (v: string = "") =>
  v ? `+${v.replace(/[^\d]/g, "")}` : v;
