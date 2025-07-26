using System.Net.Http.Json;
using Beloyal.API.Test.Users;
using Beloyal.API.Types;
using Beloyal.API.Web.Auth.Confirm;
using Beloyal.API.Web.Auth.Login;
using Beloyal.API.Web.Self.GetSelf;

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

  [Fact]
  public Task Confirm() =>
    WithScope(
      async (scope, _) =>
      {
        using var client = App!.CreateClient();
        var email = new Email("qwe@gma.com");

        Assert.Equal(
          System.Net.HttpStatusCode.Unauthorized,
          (await client.GetAsync("/api/self")).StatusCode
        );

        var res = await client.PostAsJsonAsync("/api/auth", new LoginRequest(email.Value));

        var loginOk = await res.Content.ReadFromJsonAsync<LoginResponse.Ok>();
        Assert.NotNull(loginOk);

        res = await client.PostAsJsonAsync(
          "/api/auth/confirm",
          new ConfirmRequest(email.Value, loginOk.Code)
        );

        Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, (await client.GetAsync("/api/self")).StatusCode);
      }
    );

  [Fact]
  public Task AuthenticateFixture() =>
    WithScope(
      async (scope, _) =>
      {
        using var client = App!.CreateClient();

        var user = await UserFixture.User(scope);
        await AuthFixture.Authenticate(user, client);
        var r = await client.GetAsync("/api/self");
        var body = await r.Content.ReadFromJsonAsync<GetSelfResponse.Ok>();
        Assert.Equal(user.Id, body?.User.Id);
        Assert.NotNull(body?.AdminAccess?.Role);
      }
    );
}
