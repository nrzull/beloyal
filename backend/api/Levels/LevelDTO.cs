namespace Beloyal.API.Levels;

public record LevelDTO(
  Guid Id,
  DateTimeOffset CreatedAt,
  decimal CashbackPercent,
  decimal PointsCapacity,
  string? Description
)
{
  public static LevelDTO From(Level v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      CashbackPercent: v.CashbackPercent,
      PointsCapacity: v.PointsCapacity,
      Description: v.Description
    );
};
