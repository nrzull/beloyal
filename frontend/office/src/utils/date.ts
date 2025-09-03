import {
  formatWithOptions,
  formatDistanceStrictWithOptions,
} from "date-fns/fp";
import { ru } from "date-fns/locale";
import { formatDistanceToNow, differenceInCalendarDays } from "date-fns";

export const formatDate = formatWithOptions({ locale: ru }, "dd.MM.yyyy HH:mm");

export const formatDistanceStrictDate = formatDistanceStrictWithOptions({
  locale: ru,
});

export const formatMessageTimestamp = (v: string) =>
  differenceInCalendarDays(new Date(), v) > 0
    ? formatDate(v)
    : formatDistanceToNow(v, { locale: ru });
