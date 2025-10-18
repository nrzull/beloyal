using System.Text.Json;
using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Beloyal.API.Promotions.Variants;

namespace Beloyal.API.Web.Promotions.CreatePromotion;

public class CreatePromotionEndpoint(PromotionContext promotionContext)
{
  public async Task<CreatePromotionResponse> Handle(
    CompanyAccess companyAccess,
    CreatePromotionRequest req
  )
  {
    var variant = JsonSerializer.Deserialize(
      req.Variant,
      BaseVariantJsonSerializerContext.Default.BaseVariant
    );

    if (variant is null)
    {
      throw new Exception("invalid variant");
    }

    var promotion = await promotionContext.CreatePromotion(
      companyAccess.Company,
      req.Title,
      variant
    );

    return new CreatePromotionResponse.Ok(PromotionDTO.From(promotion));
  }
}
