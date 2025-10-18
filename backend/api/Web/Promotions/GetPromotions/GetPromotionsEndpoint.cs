using Beloyal.API.Companies;
using Beloyal.API.Promotions;

namespace Beloyal.API.Web.Promotions.GetPromotions;

public class GetPromotionsEndpoint(PromotionContext promotionContext)
{
  public async Task<GetPromotionsResponse> Handle(CompanyAccess companyAccess)
  {
    var promotions = await promotionContext.GetPromotions(CompanyId.From(companyAccess));

    return new GetPromotionsResponse.Ok(promotions.Select(PromotionDTO.From).ToList());
  }
}
