using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.CreateShopAccess;

public abstract record CreateShopAccessResponse
{
  public record Ok(ShopAccessDTO Data) : CreateShopAccessResponse;
}
