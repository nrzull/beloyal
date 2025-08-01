using Vogen;

namespace Beloyal.API.Shops;

[ValueObject<Guid>]
public partial struct ShopId
{
  public static ShopId From(Shop v) => From(v.Id);
}
