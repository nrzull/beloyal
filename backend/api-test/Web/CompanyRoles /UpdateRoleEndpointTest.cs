using Beloyal.API.Companies;
using Beloyal.API.Test.Companies;
using Beloyal.API.Web.CompanyRoles.UpdateRole;

namespace Beloyal.API.Test.Web.CompanyRoles;

public class UpdateRoleEndpointTest : TestLifetime
{
  [Fact]
  public Task Update() =>
    WithScope(
      async (scope, _) =>
      {
        var companyContext = scope.Get<CompanyContext>();
        var endpoint = scope.Get<UpdateRoleEndpoint>();
        var permissions = CompanyPermissions.GetAllPermissions();
        var (company, access, _) = await CompanyFixture.Company(scope);
        var role = await companyContext.CreateCompanyRole(company, "qwe", permissions);

        var result = await endpoint.Handle(access, new(role.Id, "qwe", permissions));
        Assert.IsType<UpdateRoleResponse.Ok>(result);

        if (result is UpdateRoleResponse.Ok ok)
        {
          Assert.Equal("qwe", ok.Data.Name);
          Assert.Equivalent(permissions, ok.Data.Permissions);
        }
      }
    );
}
