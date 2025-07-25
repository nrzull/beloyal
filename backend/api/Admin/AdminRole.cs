namespace Beloyal.API.Admin;

public class AdminRole
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Name { get; set; }
  public List<string> Permissions { get; set; } = [];
}
