using Beloyal.API.Chats;
using Beloyal.API.Customers;
using Beloyal.API.Mailings;
using Beloyal.API.Messengers;
using Beloyal.API.Shops;

namespace Beloyal.API.Companies;

public class Company
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Tin { get; set; }
  public required string Title { get; set; }

  public decimal CashbackPercent { get; set; } = 1;
  public decimal CashbackThreshold { get; set; } = 30;
  public decimal BonusMultiplier { get; set; } = 10;
  public TimeSpan? BonusLifetime { get; set; } = TimeSpan.FromDays(180);

  public MaxBot? MaxBot { get; set; }
  public TgBot? TgBot { get; set; }
  public List<CompanyRole> Roles { get; set; } = [];
  public List<CompanyAccess> Accesses { get; set; } = [];
  public List<Shop> Shops { get; set; } = [];
  public List<Customer> Customers { get; set; } = [];
  public List<Mailing> Mailings { get; set; } = [];
  public List<Chat> Chats { get; set; } = [];
}
