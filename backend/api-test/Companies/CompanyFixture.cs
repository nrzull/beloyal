using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Companies;

public static class CompanyFixture
{
  public static async Task<(Company, CompanyAccess, User)> Company(
    AsyncServiceScope scope,
    string email = "qwe@gma.com",
    string tin = "0123456789",
    decimal cashbackPercent = 10,
    decimal cashbackThreshold = 10
  )
  {
    var userContext = scope.Get<UserContext>();
    var companyContext = scope.Get<CompanyContext>();
    var percent = new Percent(10);

    await companyContext.CreateWhiteCompany(new(tin), DateTimeOffset.UtcNow.AddMonths(1));

    var user = await userContext.EnsureUser(new Email(email));
    (Company company, var _, CompanyAccess access) = await companyContext.CreateCompany(
      user,
      new(tin),
      new(cashbackPercent),
      new(cashbackThreshold)
    );

    return (company, access, user);
  }
}
