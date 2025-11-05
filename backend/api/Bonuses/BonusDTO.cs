namespace Beloyal.API.Bonuses;

public record BonusDTO(
  Guid Id,
  DateTimeOffset CreatedAt,
  string Type,
  decimal Amount,
  decimal CashIncome,
  decimal RawCashIncome,
  string Status,
  decimal CashbackPercent,
  string? PromotionType,
  DateTimeOffset? ScheduledAt
)
{
  public static BonusDTO From(Bonus v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      Type: v.Type.ToString(),
      Amount: v.Amount,
      CashIncome: v.CashIncome,
      RawCashIncome: v.RawCashIncome,
      Status: v.Status.ToString(),
      CashbackPercent: v.CashbackPercent,
      PromotionType: v.Promotion is { } ? v.Promotion?.Variant.GetType().Name : null,
      ScheduledAt: v.ScheduledAt
    );
};
