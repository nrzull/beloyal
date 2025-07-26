using Beloyal.API.Companies;

namespace Beloyal.API.Shops;

public class Shop
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Name { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public List<ShopAccess> Accesses { get; set; } = [];
}
