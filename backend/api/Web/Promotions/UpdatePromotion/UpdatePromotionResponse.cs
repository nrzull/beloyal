using Beloyal.API.Promotions;

namespace Beloyal.API.Web.Promotions.UpdatePromotion;

public abstract record UpdatePromotionResponse
{
  public record Ok(PromotionDTO Data) : UpdatePromotionResponse;
}
