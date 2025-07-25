namespace Beloyal.API.Companies;

public class CompanyAccessInvitation
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Email { get; set; }
  public CompanyAccessInvitationStatus Status { get; set; } = CompanyAccessInvitationStatus.Active;

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public Guid RoleId { get; set; }
  public required CompanyRole Role { get; set; }
}
