using Beloyal.API.Shops;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Companies;

public class CompanyAccess : IEnsurable
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public Guid UserId { get; set; }
  public required User User { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public Guid RoleId { get; set; }
  public required CompanyRole Role { get; set; }

  public List<ShopAccess> ShopAccesses { get; set; } = [];
}
