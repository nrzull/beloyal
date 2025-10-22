import type {
  ExactCountVariant,
  DatePeriodVariant,
  TimePeriodVariant,
  DateTimePeriodVariant,
  PromotionVariant,
  OperationsInactivityVariant,
} from "@beloyal/shared";
import { serialize } from "tinyduration";

export const newExactCountVariant = (): ExactCountVariant => ({
  $type: "ExactCountVariant",
  cashbackPercent: 1,
  count: 1,
});

export const newDatePeriodVariant = (): DatePeriodVariant => ({
  $type: "DatePeriodVariant",
  cashbackPercent: 1,
  from: null,
  to: null,
});

export const newTimePeriodVariant = (): TimePeriodVariant => ({
  $type: "TimePeriodVariant",
  cashbackPercent: 1,
  from: null,
  to: null,
});

export const newDateTimePeriodVariant = (): DateTimePeriodVariant => ({
  $type: "DateTimePeriodVariant",
  cashbackPercent: 1,
  from: null,
  to: null,
});

export const newOperationsInactivityVariant =
  (): OperationsInactivityVariant => ({
    $type: "OperationsInactivityVariant",
    cashbackPercent: 1,
    period: serialize({ days: 1, hours: 0, minutes: 0 }),
  });

export const translatePromotionVariantType: Record<
  PromotionVariant["$type"],
  string
> = {
  DatePeriodVariant: "Дата акции",
  DateTimePeriodVariant: "Время и дата акции",
  ExactCountVariant: "Операция по счету",
  TimePeriodVariant: "Время акции",
  OperationsInactivityVariant: "Период неактивности",
};
