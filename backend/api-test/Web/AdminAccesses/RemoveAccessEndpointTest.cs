using Beloyal.API.Admin;
using Beloyal.API.Test.Users;
using Beloyal.API.Web.AdminAccesses.RemoveAccess;

namespace Beloyal.API.Test.Web.AdminAccesses;

public class RemoveAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Remove() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<RemoveAccessEndpoint>();
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        var user = await UserFixture.User(scope, "qweqwe@gma.com");
        await adminContext.CreateAdminAccess(user, role);

        var result = await endpoint.Handle(new(user.Id));
        Assert.IsType<RemoveAccessResponse.Ok>(result);

        var access = await adminContext.GetAccess(user);
        Assert.Null(access);
      }
    );
}
