using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.RemoveShopAccess;

public abstract record RemoveShopAccessResponse
{
  public record Ok(ShopAccessDTO Data) : RemoveShopAccessResponse;
}
