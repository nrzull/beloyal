using Beloyal.API.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Types;

namespace Beloyal.API.Test.Companies;

public class CompanyContextTest : TestLifetime
{
  [Fact]
  public Task CreateCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var user = await UserFixture.User(scope);
        var companyContext = scope.Get<CompanyContext>();
        var tin = new Tin("0123456789");

        await Assert.ThrowsAnyAsync<Exception>(() => companyContext.CreateCompany(user, tin));

        await companyContext.CreateWhiteCompany(tin, DateTimeOffset.UtcNow.AddMonths(1));

        (Company company, var _, CompanyAccess access) = await companyContext.CreateCompany(
          user,
          tin
        );

        Assert.Equal(company.Tin, tin.Value);
        Assert.Equal(user.Id, access.UserId);
      }
    );

  [Fact]
  public Task OnlyUniqueCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        await Assert.ThrowsAnyAsync<Exception>(() => CompanyFixture.Company(scope));
      }
    );
}
