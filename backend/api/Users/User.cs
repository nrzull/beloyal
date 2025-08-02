using Beloyal.API.Admin;
using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Users;

public class User
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? MaxUserId { get; set; }
  public string? TgUserId { get; set; }

  public AdminAccess? AdminAccess { get; set; }
  public List<CompanyAccess> CompanyAccesses { get; set; } = [];
  public List<Customer> Customers { get; set; } = [];
}
