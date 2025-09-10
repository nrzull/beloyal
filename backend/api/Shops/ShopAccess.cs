using Beloyal.API.Companies;
using Beloyal.API.Types;

namespace Beloyal.API.Shops;

public class ShopAccess : IEnsurable
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public Guid ShopId { get; set; }
  public required Shop Shop { get; set; }

  public Guid CompanyAccessId { get; set; }
  public required CompanyAccess CompanyAccess { get; set; }
}
