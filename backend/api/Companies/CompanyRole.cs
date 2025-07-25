namespace Beloyal.API.Companies;

public class CompanyRole
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Name { get; set; }
  public required List<string> Permissions { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }
}
