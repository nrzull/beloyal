using Beloyal.API.Companies;

namespace Beloyal.API.Items;

public class Item
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public string? ExternalId { get; set; }
  public required string Name { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public Guid? ParentId { get; set; }
  public ItemGroup? Parent { get; set; }
}
