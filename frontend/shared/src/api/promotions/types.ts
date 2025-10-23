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
  | TimePeriodVariant
  | MoneySpentVariant
  | OperationsInactivityVariant;

export type MoneySpentVariant = {
  $type: "MoneySpentVariant";
  amount: number;
  period?: string;
  cashbackPercent: number;
};

export type OperationsInactivityVariant = {
  $type: "OperationsInactivityVariant";
  period: string;
  cashbackPercent: number;
};

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
  from: string | null;
  to: string | null;
  cashbackPercent: number;
};
