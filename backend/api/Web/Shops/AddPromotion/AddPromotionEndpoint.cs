using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.AddPromotion;

public class AddPromotionEndpoint(ShopContext shopContext, PromotionContext promotionContext)
{
  public async Task<AddPromotionResponse> Handle(
    CompanyAccess companyAccess,
    ShopId shopId,
    PromotionId promotionId
  )
  {
    var shop = await shopContext.GetShop(CompanyId.From(companyAccess), shopId);
    if (shop is null)
    {
      throw new Exception("invalid shop");
    }

    var promotion = await promotionContext.GetPromotion(CompanyId.From(companyAccess), promotionId);
    if (promotion is null)
    {
      throw new Exception("invalid promotion");
    }

    var shopPromotion = await shopContext.AddPromotion(shop, promotion);

    return new AddPromotionResponse.Ok(ShopPromotionDTO.From(shopPromotion));
  }
}
