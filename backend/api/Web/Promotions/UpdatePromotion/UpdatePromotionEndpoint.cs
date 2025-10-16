using System.Text.Json;
using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Beloyal.API.Promotions.Variants;
using Beloyal.API.Web.Promotions.CreatePromotion;

namespace Beloyal.API.Web.Promotions.UpdatePromotion;

public class UpdatePromotionEndpoint(PromotionContext promotionContext)
{
  public async Task<UpdatePromotionResponse> Handle(
    CompanyAccess companyAccess,
    PromotionId promotionId,
    CreatePromotionRequest req
  )
  {
    var promotion = await promotionContext.GetPromotion(CompanyId.From(companyAccess), promotionId);

    if (promotion is null)
    {
      throw new Exception("invalid promotion");
    }

    var variant = JsonSerializer.Deserialize(
      req.Variant,
      BaseVariantJsonSerializerContext.Default.BaseVariant
    );

    if (variant is null)
    {
      throw new Exception("invalid variant");
    }

    await promotionContext.UpdatePromotion(promotion, req.Title, variant);

    return new UpdatePromotionResponse.Ok(PromotionDTO.From(promotion));
  }
}
