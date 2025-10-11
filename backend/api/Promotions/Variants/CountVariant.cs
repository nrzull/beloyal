namespace Beloyal.API.Promotions.Variants;

public class CountVariant : BaseVariant
{
  public ushort Count { get; set; }
  public required decimal CashbackPercent { get; set; }
}
