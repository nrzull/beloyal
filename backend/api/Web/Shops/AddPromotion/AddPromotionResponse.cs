using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.AddPromotion;

public abstract record AddPromotionResponse
{
  public record Ok(ShopPromotionDTO Data) : AddPromotionResponse;
}
