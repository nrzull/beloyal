namespace Beloyal.API.Promotions.Variants;

public class TimePeriodVariant : BaseVariant
{
  public required TimeOnly From { get; set; }
  public required TimeOnly To { get; set; }
  public override required decimal CashbackPercent { get; set; }
}
