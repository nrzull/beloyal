using Beloyal.API.Companies;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Web.CompanyAccesses.CreateAccess;

namespace Beloyal.API.Test.Web.CompanyAccesses;

public class CreateAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<CreateAccessEndpoint>();
        var companyContext = scope.Get<CompanyContext>();
        var (company, access, _) = await CompanyFixture.Company(scope);
        var role = await companyContext.CreateCompanyRole(company, "qwe", []);
        var user = await UserFixture.User(scope, "qweqwe@gma.com");

        var result = await endpoint.Handle(access, new(user.Id, role.Id));
        Assert.IsType<CreateAccessResponse.Ok>(result);

        if (result is CreateAccessResponse.Ok ok)
        {
          access = await companyContext.GetAccess(ok.Data.Id);
          Assert.NotNull(access);

          Assert.Equal(ok.Data.Role.Id, role.Id);
          Assert.Equal(access.Id, ok.Data.Id);
        }
      }
    );
}
