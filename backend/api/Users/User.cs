using Beloyal.API.Admin;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Types;

namespace Beloyal.API.Users;

public class User : IEnsurable
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? VerifiedAt { get; set; }

  public required string Phone { get; set; }

  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? MiddleName { get; set; }
  public string? MaxUserId { get; set; }
  public string? TgUserId { get; set; }

  public AdminAccess? AdminAccess { get; set; }
  public List<CompanyAccess> CompanyAccesses { get; set; } = [];
  public List<Customer> Customers { get; set; } = [];
}
