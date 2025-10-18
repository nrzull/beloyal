using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetPromotions;

public abstract record GetPromotionsResponse
{
  public record Ok(List<ShopPromotionDTO> Data) : GetPromotionsResponse;
}
