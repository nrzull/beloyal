using Beloyal.API.Companies;
using Beloyal.API.Promotions;

namespace Beloyal.API.Web.Promotions.GetPromotion;

public class GetPromotionEndpoint(PromotionContext promotionContext)
{
  public async Task<GetPromotionResponse> Handle(
    CompanyAccess companyAccess,
    PromotionId promotionId
  )
  {
    var promotion = await promotionContext.GetPromotion(CompanyId.From(companyAccess), promotionId);

    if (promotion is null)
    {
      return new GetPromotionResponse.NotFound();
    }

    return new GetPromotionResponse.Ok(PromotionDTO.From(promotion));
  }
}
