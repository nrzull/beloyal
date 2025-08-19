import {
  formatWithOptions,
  formatDistanceStrictWithOptions,
} from "date-fns/fp";
import { ru } from "date-fns/locale";

export const formatDate = formatWithOptions({ locale: ru }, "dd.MM.yyyy HH:mm");

export const formatDistanceStrictDate = formatDistanceStrictWithOptions({
  locale: ru,
});
