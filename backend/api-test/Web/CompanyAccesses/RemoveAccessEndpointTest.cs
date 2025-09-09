using Beloyal.API.Companies;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Web.CompanyAccesses.RemoveAccess;

namespace Beloyal.API.Test.Web.CompanyAccesses;

public class RemoveAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Remove() =>
    WithScope(
      async (scope, _) =>
      {
        var companyContext = scope.Get<CompanyContext>();
        var endpoint = scope.Get<RemoveAccessEndpoint>();
        var (company, _, _) = await CompanyFixture.Company(scope);

        var role = await companyContext.CreateCompanyRole(company, "qwe", []);
        var user = await UserFixture.User(scope, "+12345678901");
        var access = await companyContext.CreateCompanyAccess(company, user, role);

        var result = await endpoint.Handle(access, new(access.Id));

        Assert.IsType<RemoveAccessResponse.Ok>(result);

        access = await companyContext.GetAccess(CompanyAccessId.From(access.Id));
        Assert.Null(access);
      }
    );
}
