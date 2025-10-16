using Beloyal.API.Promotions;

namespace Beloyal.API.Web.Promotions.GetPromotion;

public abstract record GetPromotionResponse
{
  public record NotFound : GetPromotionResponse;

  public record Ok(PromotionDTO Data) : GetPromotionResponse;
}
