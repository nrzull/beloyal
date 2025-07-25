using Beloyal.API.Admin;
using Beloyal.API.Companies;

namespace Beloyal.API.Users;

public class User
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public required string Email { get; set; }

  public AdminAccess? AdminAccess { get; set; }
  public List<CompanyAccess> CompanyAccesses { get; set; } = [];
}
