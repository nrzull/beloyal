using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.RemoveShopAccess;

public class RemoveShopAccessEndpoint(ShopContext shopContext)
{
  public async Task<RemoveShopAccessResponse> Handle(
    CompanyAccess companyAccess,
    ShopAccessId shopAccessId
  )
  {
    var shopAccess = await shopContext.GetAccess(shopAccessId);
    if (shopAccess is null || shopAccess.Shop.CompanyId != companyAccess.CompanyId)
    {
      throw new Exception("invalid shop access");
    }

    await shopContext.RemoveAccess(shopAccess);

    return new RemoveShopAccessResponse.Ok(ShopAccessDTO.From(shopAccess));
  }
}
