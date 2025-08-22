using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetShop;

public class GetShopEndpoint(ShopContext shopContext)
{
  public async Task<GetShopResponse> Handle(CompanyAccess access, ShopId shopId)
  {
    var shop = await shopContext.GetShop(CompanyId.From(access.CompanyId), shopId);

    if (shop is null)
    {
      return new GetShopResponse.NotFound();
    }

    return new GetShopResponse.Ok(ShopDTO.From(shop));
  }
}
