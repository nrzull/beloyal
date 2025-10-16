export type Promotion = {
  id: string;
  title: string;
  createdAt: string;
  variant: PromotionVariant;
};

export type PromotionVariant =
  | ExactCountVariant
  | DatePeriodVariant
  | DateTimePeriodVariant
  | TimePeriodVariant;

export type ExactCountVariant = {
  $type: "ExactCountVariant";
  count: number;
  cashbackPercent: number;
};

export type DatePeriodVariant = {
  $type: "DatePeriodVariant";
  from: string | null;
  to: string | null;
  cashbackPercent: number;
};

export type DateTimePeriodVariant = {
  $type: "DateTimePeriodVariant";
  from: string | null;
  to: string | null;
  cashbackPercent: number;
};

export type TimePeriodVariant = {
  $type: "TimePeriodVariant";
  from: number | null;
  to: number | null;
  cashbackPercent: number;
};
