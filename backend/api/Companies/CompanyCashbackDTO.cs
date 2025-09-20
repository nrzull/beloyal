namespace Beloyal.API.Companies;

public record CompanyCashbackDTO(
  decimal CashbackPercent,
  decimal CashbackThreshold,
  decimal BonusMultiplier,
  uint BonusLifetimeInDays
)
{
  public static CompanyCashbackDTO From(Company v) =>
    new(
      CashbackPercent: v.CashbackPercent,
      CashbackThreshold: v.CashbackThreshold,
      BonusMultiplier: v.BonusMultiplier,
      BonusLifetimeInDays: (uint)v.BonusLifetime.TotalDays
    );
};
