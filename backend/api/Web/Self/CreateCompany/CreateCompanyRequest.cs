namespace Beloyal.API.Web.Self.CreateCompany;

public record CreateCompanyRequest(
  string Tin,
  decimal CashbackPercent,
  decimal CashbackThreshold,
  decimal BonusMultiplier
);
