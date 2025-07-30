using Beloyal.API.Admin;
using Beloyal.API.Web.AdminRoles.UpdateRole;

namespace Beloyal.API.Test.Web.AdminRoles;

public class UpdateRoleEndpointTest : TestLifetime
{
  [Fact]
  public Task Update() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var endpoint = scope.Get<UpdateRoleEndpoint>();
        var permissions = AdminPermissions.GetAllPermissions();
        var role = await adminContext.CreateAdminRole("qwe", permissions);

        var result = await endpoint.Handle(new(role.Id, "qwe", permissions));
        Assert.IsType<UpdateRoleResponse.Ok>(result);

        if (result is UpdateRoleResponse.Ok ok)
        {
          Assert.Equal("qwe", ok.Data.Name);
          Assert.Equivalent(permissions, ok.Data.Permissions);
        }
      }
    );
}
