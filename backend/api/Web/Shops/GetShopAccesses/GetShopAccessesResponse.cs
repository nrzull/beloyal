using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetShopAccesses;

public abstract record GetShopAccessesResponse
{
  public record Ok(List<ShopAccessDTO> Data) : GetShopAccessesResponse;
}
