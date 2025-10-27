using Beloyal.API.Bonuses;
using Beloyal.API.Customers;

namespace Beloyal.API.Boosters;

public class Booster
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public required DateTimeOffset ExpiredAt { get; set; }

  public required decimal CashbackPercent { get; set; }

  public Bonus? Bonus { get; set; }

  public Guid CustomerId { get; set; }
  public required Customer Customer { get; set; }
}
