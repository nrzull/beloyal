using Vogen;

namespace Beloyal.API.Levels;

[ValueObject<Guid>]
public partial struct LevelId
{
  public static LevelId From(Level v) => From(v.Id);
};
