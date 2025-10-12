import type {
  ExactCountVariant,
  DatePeriodVariant,
  TimePeriodVariant,
  DateTimePeriodVariant,
  PromotionVariant,
} from "@beloyal/shared";

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

export const translatePromotionVariantType: Record<
  PromotionVariant["$type"],
  string
> = {
  DatePeriodVariant: "Дата акции",
  DateTimePeriodVariant: "Время и дата акции",
  ExactCountVariant: "Операция по счету",
  TimePeriodVariant: "Время акции",
};
