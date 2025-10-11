namespace Beloyal.API.Promotions.Variants;

public class DatePeriodVariant : BaseVariant
{
  public required DateOnly From { get; set; }
  public required DateOnly To { get; set; }
  public required decimal CashbackPercent { get; set; }
}
