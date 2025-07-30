using Beloyal.API.Admin;
using Beloyal.API.Web.AdminRoles.CreateRole;

namespace Beloyal.API.Test.Web.AdminRoles;

public class CreateRoleEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<CreateRoleEndpoint>();
        var permissions = AdminPermissions.GetAllPermissions();

        var result = await endpoint.Handle(new("qwe", permissions));
        Assert.IsType<CreateRoleResponse.Ok>(result);

        if (result is CreateRoleResponse.Ok ok)
        {
          Assert.Equal("qwe", ok.Data.Name);
          Assert.Equivalent(permissions, ok.Data.Permissions);
        }
      }
    );
}
