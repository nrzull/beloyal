using Beloyal.API.Companies;

namespace Beloyal.API.Levels;

public class Level
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public string? Description { get; set; }
  public required decimal CashbackPercent { get; set; }
  public required decimal PointsCapacity { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }
}
