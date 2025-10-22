namespace Beloyal.API.Promotions.Variants;

public class OperationsInactivityVariant : BaseVariant
{
  public required string Period { get; set; }
  public override required decimal CashbackPercent { get; set; }
}
