using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.UpdateShop;

public class UpdateShopEndpoint(ShopContext shopContext)
{
  public async Task<UpdateShopResponse> Handle(CompanyAccess access, UpdateShopRequest req)
  {
    var shop = await shopContext.GetShop(CompanyId.From(access), ShopId.From(req.Id));
    if (shop is null)
    {
      throw new Exception("invalid shop");
    }

    await shopContext.UpdateShop(shop, req.Name);

    return new UpdateShopResponse.Ok(ShopDTO.From(shop));
  }
}
