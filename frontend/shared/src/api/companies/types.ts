export type Company = {
  id: string;
  createdAt: string;
  title: string;
  tin: string;
};

export type CompanyBonusOptions = {
  cashbackPercent: number;
  cashbackThreshold: number;
  bonusMultiplier: number;
  bonusLifetimeInDays: number;
  bonusFrequencyCount?: number;
  bonusFrequencyInDays?: number;
  bonusScheduling: string | null;
};

export type CompanyLink = {
  type: "telegram";
  url: string;
};
