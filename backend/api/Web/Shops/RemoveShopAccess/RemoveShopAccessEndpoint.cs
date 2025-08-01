using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.RemoveShopAccess;

public class RemoveShopAccessEndpoint(ShopContext shopContext)
{
  public async Task<RemoveShopAccessResponse> Handle(RemoveShopAccessRequest req)
  {
    var shopAccess = await shopContext.GetAccess(ShopAccessId.From(req.ShopAccessId));
    if (shopAccess is null)
    {
      throw new Exception("invalid shop access");
    }

    await shopContext.RemoveAccess(shopAccess);

    return new RemoveShopAccessResponse.Ok(ShopAccessDTO.From(shopAccess));
  }
}
