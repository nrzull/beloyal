namespace Beloyal.API.Promotions.Variants;

public class DateTimePeriodVariant : BaseVariant
{
  public DateTimeOffset From { get; set; }
  public DateTimeOffset To { get; set; }
  public required decimal CashbackPercent { get; set; }
}
