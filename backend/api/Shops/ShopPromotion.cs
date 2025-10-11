using Beloyal.API.Promotions;

namespace Beloyal.API.Shops;

public class ShopPromotion
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public Guid ShopId { get; set; }
  public required Shop Shop { get; set; }

  public Guid PromotionId { get; set; }
  public required Promotion Promotion { get; set; }
}
