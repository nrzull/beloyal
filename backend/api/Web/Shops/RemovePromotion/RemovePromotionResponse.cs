using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.RemovePromotion;

public abstract record RemovePromotionResponse
{
  public record Ok(ShopPromotionDTO Data) : RemovePromotionResponse;
}
