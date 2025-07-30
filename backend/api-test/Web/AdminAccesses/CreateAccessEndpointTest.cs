using Beloyal.API.Admin;
using Beloyal.API.Test.Users;
using Beloyal.API.Web.AdminAccesses.CreateAccess;

namespace Beloyal.API.Test.Web.AdminAccesses;

public class CreateAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<CreateAccessEndpoint>();
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        var user = await UserFixture.User(scope, "qweqwe@gma.com");

        var result = await endpoint.Handle(new(user.Id, role.Id));
        var access = await adminContext.GetAccess(user);

        Assert.NotNull(access);

        Assert.IsType<CreateAccessResponse.Ok>(result);
        if (result is CreateAccessResponse.Ok ok)
        {
          Assert.Equal(ok.Data.Role.Id, role.Id);
          Assert.Equal(access.Id, ok.Data.Id);
        }
      }
    );
}
