namespace Beloyal.API.Companies;

public class CompanyRole
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

  public required string Name { get; set; }
  public List<string> Permissions { get; set; } = [];

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }
}
