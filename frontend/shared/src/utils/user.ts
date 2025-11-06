import type { User } from "../api";

export const buildFullName = (v?: {
  firstName?: string;
  lastName?: string;
  middleName?: string;
}) => {
  if (!v) {
    return "";
  }

  const { firstName: first, lastName: last, middleName: middle } = v;

  if (first && last && middle) {
    return `${last} ${first.charAt(0)}. ${middle.charAt(0)}.`;
  }

  return [last, first, middle].filter((v) => v).join(" ");
};
