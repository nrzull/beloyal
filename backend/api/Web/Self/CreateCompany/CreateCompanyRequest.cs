namespace Beloyal.API.Web.Self.CreateCompany;

public record CreateCompanyRequest(
  string Tin,
  string Title,
  decimal CashbackPercent,
  decimal CashbackThreshold,
  decimal BonusMultiplier,
  uint BonusLifetimeInDays
);
