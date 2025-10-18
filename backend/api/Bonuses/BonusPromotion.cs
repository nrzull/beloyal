using Beloyal.API.Promotions;
using Beloyal.API.Promotions.Variants;

namespace Beloyal.API.Bonuses;

public class BonusPromotion
{
  public required string Title { get; set; }
  public required BaseVariant Variant { get; set; }

  public static BonusPromotion From(Promotion v) =>
    new BonusPromotion { Title = v.Title, Variant = v.Variant };
}
