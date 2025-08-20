import { isPlainObject, pathOr, stringToPath } from "remeda";

export const extract = (v: unknown, path?: string) => {
  if (!path) {
    return v;
  }

  if (isPlainObject(v)) {
    return pathOr(v, stringToPath(path), v);
  }

  return v;
};
