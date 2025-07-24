using Beloyal.API.Companies;
using Beloyal.API.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Companies;

public static class CompanyFixture
{
  public static async Task<(Company, CompanyAccess, User)> Company(
    AsyncServiceScope scope,
    string email = "qwe@gma.com",
    string tin = "0123456789"
  )
  {
    var userContext = scope.Get<UserContext>();
    var companyContext = scope.Get<CompanyContext>();

    var user = await userContext.EnsureUser(new(email));
    (Company company, var _, CompanyAccess access) = await companyContext.CreateCompany(
      user,
      new(tin)
    );

    return (company, access, user);
  }
}
