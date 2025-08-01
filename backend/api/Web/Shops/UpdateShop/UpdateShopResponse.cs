using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.UpdateShop;

public abstract record UpdateShopResponse
{
  public record Ok(ShopDTO Data) : UpdateShopResponse;
}
