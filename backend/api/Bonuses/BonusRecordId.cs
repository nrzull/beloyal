using Vogen;

namespace Beloyal.API.Bonuses;

[ValueObject<Guid>]
public partial struct BonusRecordId
{
  public static BonusRecordId From(BonusRecord v) => From(v.Id);
};
