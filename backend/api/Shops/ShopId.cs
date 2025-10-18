using Vogen;

namespace Beloyal.API.Shops;

[ValueObject<Guid>]
public partial struct ShopId
{
  public static ShopId From(Shop v) => From(v.Id);

  public static ShopId From(ShopAccess v) => From(v.ShopId);
}
