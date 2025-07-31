using Beloyal.API.Companies;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Web.CompanyAccesses.UpdateAccess;

namespace Beloyal.API.Test.Web.CompanyAccesses;

public class UpdateAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Update() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<UpdateAccessEndpoint>();
        var companyContext = scope.Get<CompanyContext>();
        var (company, access, _) = await CompanyFixture.Company(scope);
        var role = await companyContext.CreateCompanyRole(company, "qwe", []);
        var role2 = await companyContext.CreateCompanyRole(company, "qwe", []);

        var user = await UserFixture.User(scope, "qweqwe@gma.com");

        await Assert.ThrowsAnyAsync<Exception>(() =>
          endpoint.Handle(access, new(user.Id, role.Id))
        );

        access = await companyContext.CreateCompanyAccess(company, user, role);

        var result = await endpoint.Handle(access, new(access.Id, role2.Id));
        Assert.IsType<UpdateAccessResponse.Ok>(result);

        if (result is UpdateAccessResponse.Ok ok)
        {
          Assert.Equal(ok.Data.Role.Id, role2.Id);
        }
      }
    );
}
