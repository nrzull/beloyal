using System.Net.Http.Json;
using Beloyal.API.Admin;
using Beloyal.API.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Test.Web.Auth;
using Beloyal.API.Web.Self.GetSelf;

namespace Beloyal.API.Test.Web.Self;

public class SelfControllerTest : TestLifetime
{
  [Fact]
  public Task GetSelf() =>
    WithScope(
      async (scope, _) =>
      {
        using var client = App!.CreateClient();
        var user = await UserFixture.User(scope);
        await AuthFixture.Authenticate(user, client);

        var r = await client.GetAsync("/api/self");
        var body = await r.Content.ReadFromJsonAsync<GetSelfResponse.Ok>();

        Assert.Equal(user.Id, body?.User.Id);
        Assert.IsType<AdminRoleDTO>(body?.AdminAccess?.Role);
        Assert.IsType<List<CompanyAccessDTO>>(body?.CompanyAccesses);
      }
    );
}
