export type Company = {
  id: string;
  createdAt: string;
  title: string;
  tin: string;
};

export type CompanyCashback = {
  cashbackPercent: number;
  cashbackThreshold: number;
  bonusMultiplier: number;
  bonusLifetimeInDays: number;
};

export type CompanyLink = {
  type: "telegram";
  url: string;
};
