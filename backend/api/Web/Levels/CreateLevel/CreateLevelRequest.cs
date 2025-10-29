namespace Beloyal.API.Web.Levels.CreateLevel;

public record CreateLevelRequest(
  decimal CashbackPercent,
  decimal PointsCapacity,
  string? Description
);
