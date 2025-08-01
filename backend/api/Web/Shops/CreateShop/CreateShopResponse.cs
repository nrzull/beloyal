using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.CreateShop;

public abstract record CreateShopResponse
{
  public record Ok(ShopDTO Data) : CreateShopResponse;
}
