using Vogen;

namespace Beloyal.API.Promotions;

[ValueObject<Guid>]
public partial struct PromotionId
{
  public static PromotionId From(Promotion v) => From(v.Id);
};
