using Beloyal.API.Promotions;

namespace Beloyal.API.Web.Promotions.CreatePromotion;

public abstract record CreatePromotionResponse
{
  public record Ok(PromotionDTO Data) : CreatePromotionResponse;
}
