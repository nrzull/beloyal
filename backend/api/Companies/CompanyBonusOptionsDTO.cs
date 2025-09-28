namespace Beloyal.API.Companies;

public record CompanyBonusOptionsDTO(
  decimal CashbackPercent,
  decimal CashbackThreshold,
  decimal BonusMultiplier,
  ushort BonusLifetimeInDays,
  ushort? BonusFrequencyInDays,
  ushort? BonusFrequencyCount
)
{
  public static CompanyBonusOptionsDTO From(Company v) =>
    new(
      CashbackPercent: v.CashbackPercent,
      CashbackThreshold: v.CashbackThreshold,
      BonusMultiplier: v.BonusMultiplier,
      BonusLifetimeInDays: (ushort)v.BonusLifetime.TotalDays,
      BonusFrequencyCount: v.BonusFrequency?.Count,
      BonusFrequencyInDays: v.BonusFrequency is { }
        ? (ushort)v.BonusFrequency.Frequency.TotalDays
        : null
    );
};
