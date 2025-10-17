using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetPromotions;

public class GetPromotionsEndpoint(ShopContext shopContext)
{
  public async Task<GetPromotionsResponse> Handle(CompanyAccess companyAccess, ShopId shopId)
  {
    var promotions = await shopContext.GetPromotions(CompanyId.From(companyAccess), shopId);

    return new GetPromotionsResponse.Ok(promotions.Select(ShopPromotionDTO.From).ToList());
  }
}
