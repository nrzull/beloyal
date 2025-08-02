using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Messengers;

public class MaxBot
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public required string Token { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public List<Customer> Customers { get; set; } = [];
}
