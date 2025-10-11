namespace Beloyal.API.Promotions.Variants;

public class TimePeriodVariant : BaseVariant
{
  public TimeOnly From { get; set; }
  public TimeOnly To { get; set; }
  public required decimal CashbackPercent { get; set; }
}
