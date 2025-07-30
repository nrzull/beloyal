using Beloyal.API.Companies;
using Beloyal.API.Test.Companies;
using Beloyal.API.Web.CompanyRoles.CreateRole;

namespace Beloyal.API.Test.Web.CompanyRoles;

public class CreateRoleEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<CreateRoleEndpoint>();
        var permissions = CompanyPermissions.GetAllPermissions();
        var (company, access, _) = await CompanyFixture.Company(scope);

        var result = await endpoint.Handle(access, new("qwe", permissions));
        Assert.IsType<CreateRoleResponse.Ok>(result);

        if (result is CreateRoleResponse.Ok ok)
        {
          Assert.Equal("qwe", ok.Data.Name);
          Assert.Equivalent(permissions, ok.Data.Permissions);
        }
      }
    );
}
