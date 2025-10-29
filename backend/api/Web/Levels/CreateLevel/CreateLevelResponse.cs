using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.CreateLevel;

public abstract record CreateLevelResponse
{
  public record Ok(LevelDTO Data) : CreateLevelResponse;
}
