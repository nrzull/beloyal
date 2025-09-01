using Vogen;

namespace Beloyal.API.Bonuses;

[ValueObject<Guid>]
public partial struct BonusId
{
  public static BonusId From(Bonus v) => From(v.Id);
};
