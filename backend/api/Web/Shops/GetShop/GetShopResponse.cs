using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetShop;

public abstract record GetShopResponse
{
  public record NotFound : GetShopResponse;

  public record Ok(ShopDTO Data) : GetShopResponse;
}
