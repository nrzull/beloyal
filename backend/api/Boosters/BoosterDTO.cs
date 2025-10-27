namespace Beloyal.API.Boosters;

public record BoosterDTO(
  Guid Id,
  DateTimeOffset CreatedAt,
  DateTimeOffset ExpiredAt,
  decimal CashbackPercent,
  bool IsActive
)
{
  public static BoosterDTO From(Booster v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      ExpiredAt: v.ExpiredAt,
      CashbackPercent: v.CashbackPercent,
      IsActive: v.ExpiredAt < v.CreatedAt ? false
        : v.Bonus?.Id is { } ? false
        : true
    );
};
