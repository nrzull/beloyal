namespace Beloyal.API.Promotions.Variants;

public class DateTimePeriodVariant : BaseVariant
{
  public required DateTime From { get; set; }
  public required DateTime To { get; set; }
  public override required decimal CashbackPercent { get; set; }
}
