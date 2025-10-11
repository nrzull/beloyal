export type Promotion = {
  id: string;
  title: string;
  createdAt: string;
  variant: PromotionVariant;
};

export type PromotionVariant =
  | CountVariant
  | DatePeriodVariant
  | DateTimePeriodVariant
  | TimePeriodVariant;

export type CountVariant = {
  $type: "CountVariant";
  count: number;
  cashbackPercent: number;
};

export type DatePeriodVariant = {
  $type: "DatePeriodVariant";
  from: string;
  to: string;
  cashbackPercent: number;
};

export type DateTimePeriodVariant = {
  $type: "DateTimePeriodVariant";
  from: string;
  to: string;
  cashbackPercent: number;
};

export type TimePeriodVariant = {
  $type: "TimePeriodVariant";
  from: string;
  to: string;
  cashbackPercent: number;
};
