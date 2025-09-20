namespace Beloyal.API.Bonuses;

public record BonusDTO(
  Guid Id,
  DateTimeOffset CreatedAt,
  string Type,
  decimal Amount,
  decimal CashIncome
)
{
  public static BonusDTO From(Bonus v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      Type: v.Type.ToString(),
      Amount: v.Amount,
      CashIncome: v.CashIncome
    );
};
