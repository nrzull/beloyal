using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.GetLevels;

public abstract record GetLevelsResponse
{
  public record Ok(List<LevelDTO> Data) : GetLevelsResponse;
}
