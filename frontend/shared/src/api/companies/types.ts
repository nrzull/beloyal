export type Company = {
  id: string;
  createdAt: string;
  title: string;
  tin: string;
  timeZone: string;
  countryCode: string;
  countryLangs: string[];
};

export type CompanyBonusOptions = {
  cashbackPercent: number;
  cashbackThreshold: number;
  bonusMultiplier: number;
  bonusLifetimeInDays: number;
  bonusFrequencyCount?: number;
  bonusFrequencyInDays?: number;
  bonusScheduling: string | null;
  welcomeBonus?: number;
  levelLifetimeInDays: number;
};

export type CompanyLink = {
  type: "telegram";
  url: string;
};
