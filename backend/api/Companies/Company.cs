namespace Beloyal.API.Companies;

public class Company
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

  public required string Tin { get; set; }

  public List<CompanyRole> Roles { get; set; } = [];
  public List<CompanyAccess> Accesses { get; set; } = [];
  public List<CompanyAccessInvitation> AccessInvitations { get; set; } = [];
}
