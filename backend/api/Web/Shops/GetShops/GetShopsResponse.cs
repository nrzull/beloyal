using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetShops;

public abstract record GetShopsResponse
{
  public record Ok(List<ShopDTO> Data) : GetShopsResponse;
}
