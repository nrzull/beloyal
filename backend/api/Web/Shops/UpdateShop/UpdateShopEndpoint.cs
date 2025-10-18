using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.UpdateShop;

public class UpdateShopEndpoint(ShopContext shopContext)
{
  public async Task<UpdateShopResponse> Handle(
    CompanyAccess access,
    ShopId shopId,
    UpdateShopRequest req
  )
  {
    var shop = await shopContext.GetShop(CompanyId.From(access), shopId);
    if (shop is null)
    {
      throw new Exception("invalid shop");
    }

    await shopContext.UpdateShop(
      shop,
      req.Name,
      req.TimeZone is { } ? TimeZoneInfo.FindSystemTimeZoneById(req.TimeZone) : null
    );

    return new UpdateShopResponse.Ok(ShopDTO.From(shop));
  }
}
