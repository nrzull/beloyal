using Vogen;

namespace Beloyal.API.Items;

[ValueObject<Guid>]
public partial struct ItemGroupId
{
  public static ItemGroupId From(ItemGroup v) => From(v.Id);
};
