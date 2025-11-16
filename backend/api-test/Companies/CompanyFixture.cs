using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Companies;

public static class CompanyFixture
{
  public static async Task<(Company, CompanyAccess, User)> Company(
    AsyncServiceScope scope,
    string phone = "+123123123123",
    string tin = "0123456789",
    decimal cashbackPercent = 10,
    decimal cashbackThreshold = 10,
    decimal bonusMultiplier = 1
  )
  {
    var userContext = scope.Get<UserContext>();
    var companyContext = scope.Get<CompanyContext>();

    await companyContext.CreateWhiteCompany(new(tin), DateTimeOffset.UtcNow.AddMonths(1));

    var user = await userContext.EnsureUser(new Phone(phone));
    (Company company, var _, CompanyAccess access) = await companyContext.CreateCompany(
      user,
      new(tin),
      "title",
      new(cashbackPercent),
      new(cashbackThreshold),
      new(bonusMultiplier),
      TimeSpan.FromDays(1),
      TimeZoneInfo.Local,
      TimeSpan.FromDays(1),
      Countries.CountryCode.RU
    );

    return (company, access, user);
  }
}
