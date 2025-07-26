using System.Net.Http.Json;
using Beloyal.API.Web.Auth.Login;

namespace Beloyal.API.Test.Web.Auth;

public class AuthControllerTest : TestLifetime
{
  [Fact]
  public Task Login() =>
    WithScope(
      async (scope, _) =>
      {
        using var client = App!.CreateClient();

        var resp = await client.PostAsJsonAsync("/api/auth", new LoginRequest("qwe@gma.com"));

        var content = await resp.Content.ReadFromJsonAsync<LoginResponse.Ok>();

        Assert.NotNull(content?.ExpiredAt);
        Assert.NotNull(content.Code);
      }
    );
}
