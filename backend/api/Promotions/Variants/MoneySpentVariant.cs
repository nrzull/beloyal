namespace Beloyal.API.Promotions.Variants;

public class MoneySpentVariant : BaseVariant
{
  public required decimal Amount { get; set; }
  public string? Period { get; set; }
  public override required decimal CashbackPercent { get; set; }
}
