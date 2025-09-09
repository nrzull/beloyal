using Beloyal.API.Admin;
using Beloyal.API.Test.Users;
using Beloyal.API.Web.AdminAccesses.UpdateAccess;

namespace Beloyal.API.Test.Web.AdminAccesses;

public class UpdateAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Update() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<UpdateAccessEndpoint>();
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        var role2 = await adminContext.CreateAdminRole("qwe", []);
        var user = await UserFixture.User(scope, "+12345678901");

        await Assert.ThrowsAnyAsync<Exception>(() => endpoint.Handle(new(user.Id, role.Id)));

        await adminContext.CreateAdminAccess(user, role);

        var result = await endpoint.Handle(new(user.Id, role2.Id));
        Assert.IsType<UpdateAccessResponse.Ok>(result);

        if (result is UpdateAccessResponse.Ok ok)
        {
          Assert.Equal(ok.Data.Role.Id, role2.Id);
        }
      }
    );
}
