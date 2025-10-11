using Beloyal.API.Companies;
using Beloyal.API.Promotions.Variants;
using Beloyal.API.Shops;

namespace Beloyal.API.Promotions;

public class Promotion
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Title { get; set; }
  public required BaseVariant Variant { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public List<Shop> Shops { get; set; } = [];
}
