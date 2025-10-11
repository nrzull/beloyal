using Beloyal.API.Promotions;

namespace Beloyal.API.Web.Promotions.GetPromotions;

public abstract record GetPromotionsResponse
{
  public record Ok(List<PromotionDTO> Data) : GetPromotionsResponse;
}
