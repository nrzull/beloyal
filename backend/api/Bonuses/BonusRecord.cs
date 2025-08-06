using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Shops;

namespace Beloyal.API.Bonuses;

public class BonusRecord
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required BonusRecordType Type { get; set; }
  public required BonusRecordStatus Status { get; set; }

  public decimal Income { get; set; }
  public decimal Bonus { get; set; }
  public decimal CashbackPercent { get; set; }
  public decimal CashbackThreshold { get; set; }

  public string? ConfirmationCode { get; set; }
  public DateTimeOffset? ConfirmationCodeExpiredAt { get; set; }

  public Guid CustomerId { get; set; }
  public required Customer Customer { get; set; }

  public Guid? AgentId { get; set; }
  public CompanyAccess? Agent { get; set; }

  public Guid? ShopId { get; set; }
  public Shop? Shop { get; set; }
}
