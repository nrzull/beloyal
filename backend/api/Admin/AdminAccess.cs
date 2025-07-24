using Beloyal.API.Users;

namespace Beloyal.API.Admin;

public class AdminAccess
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

  public Guid UserId { get; set; }
  public required User User { get; set; }

  public Guid RoleId { get; set; }
  public required AdminRole Role { get; set; }
}
