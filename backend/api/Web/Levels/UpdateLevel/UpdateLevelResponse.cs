using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.UpdateLevel;

public abstract record UpdateLevelResponse
{
  public record Ok(LevelDTO Data) : UpdateLevelResponse;
}
