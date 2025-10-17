using Beloyal.API.Promotions;

namespace Beloyal.API.Shops;

public record ShopPromotionDTO(Guid Id, DateTimeOffset CreatedAt, PromotionDTO Promotion)
{
  public static ShopPromotionDTO From(ShopPromotion v) =>
    new(Id: v.Id, CreatedAt: v.CreatedAt, Promotion: PromotionDTO.From(v.Promotion));
};
