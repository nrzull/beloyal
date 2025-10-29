using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.GetLevel;

public abstract record GetLevelResponse
{
  public record Ok(LevelDTO Data) : GetLevelResponse;

  public record NotFound : GetLevelResponse;
}
