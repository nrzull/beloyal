namespace Beloyal.API.Promotions.Variants;

public class ExactCountVariant : BaseVariant
{
  public required ushort Count { get; set; }
  public required decimal CashbackPercent { get; set; }
}
