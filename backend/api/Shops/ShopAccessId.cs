using Vogen;

namespace Beloyal.API.Shops;

[ValueObject<Guid>]
public partial struct ShopAccessId
{
  public static ShopAccessId From(ShopAccess v) => From(v.Id);
};
