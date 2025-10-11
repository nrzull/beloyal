using Beloyal.API.Promotions.Variants;

namespace Beloyal.API.Bonuses;

public class BonusPromotion
{
  public required string Title { get; set; }
  public required BaseVariant Variant { get; set; }
}
