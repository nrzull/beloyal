using System.Net.Http.Json;
using Beloyal.API.Types;
using Beloyal.API.Web.Auth.Confirm;
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

        var resp = await client.PostAsJsonAsync("/api/auth", new LoginRequest("+123123123123"));

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
        var phone = new Phone("+12312312331");

        Assert.Equal(
          System.Net.HttpStatusCode.Unauthorized,
          (await client.GetAsync("/api/self")).StatusCode
        );

        var res = await client.PostAsJsonAsync("/api/auth", new LoginRequest(phone.Value));

        var loginOk = await res.Content.ReadFromJsonAsync<LoginResponse.Ok>();
        Assert.NotNull(loginOk);

        res = await client.PostAsJsonAsync(
          "/api/auth/confirm",
          new ConfirmRequest(phone.Value, loginOk.Code)
        );

        Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, (await client.GetAsync("/api/self")).StatusCode);
      }
    );
}
