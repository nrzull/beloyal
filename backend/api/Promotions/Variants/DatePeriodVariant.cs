namespace Beloyal.API.Promotions.Variants;

public class DatePeriodVariant : BaseVariant
{
  public DateOnly From { get; set; }
  public DateOnly To { get; set; }
  public required decimal CashbackPercent { get; set; }
}
