namespace Beloyal.API.Promotions.Variants;

public class DateTimePeriodVariant : BaseVariant
{
  public required DateTimeOffset From { get; set; }
  public required DateTimeOffset To { get; set; }
  public required decimal CashbackPercent { get; set; }
}
