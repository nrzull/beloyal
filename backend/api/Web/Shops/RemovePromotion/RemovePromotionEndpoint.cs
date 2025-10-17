using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.RemovePromotion;

public class RemovePromotionEndpoint(ShopContext shopContext)
{
  public async Task<RemovePromotionResponse> Handle(
    CompanyAccess companyAccess,
    ShopPromotionId shopPromotionId
  )
  {
    var promotion = await shopContext.GetPromotion(CompanyId.From(companyAccess), shopPromotionId);
    if (promotion is null)
    {
      throw new Exception("invalid promotion");
    }

    await shopContext.RemovePromotion(promotion);

    return new RemovePromotionResponse.Ok(ShopPromotionDTO.From(promotion));
  }
}
