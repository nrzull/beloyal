using Beloyal.API.Companies;
using Beloyal.API.Messengers;
using Beloyal.API.Users;

namespace Beloyal.API.Customers;

public class Customer
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public Guid UserId { get; set; }
  public required User User { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public Guid? MaxBotId { get; set; }
  public MaxBot? MaxBot { get; set; }

  public Guid? TgBotId { get; set; }
  public TgBot? TgBot { get; set; }
}
