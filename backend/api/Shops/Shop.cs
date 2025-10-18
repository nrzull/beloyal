using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Beloyal.API.Types;

namespace Beloyal.API.Shops;

public class Shop : IEnsurable
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Name { get; set; }
  public TimeZoneInfo? TimeZone { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public List<ShopAccess> Accesses { get; set; } = [];
  public List<Promotion> Promotions { get; set; } = [];
}
