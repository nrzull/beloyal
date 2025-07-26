using System.Net.Http.Json;
using Beloyal.API.Users;
using Beloyal.API.Web.Auth.Confirm;
using Beloyal.API.Web.Auth.Login;

namespace Beloyal.API.Test.Web.Auth;

public static class AuthFixture
{
  public static async Task Authenticate(User user, HttpClient client)
  {
    var res = await client.PostAsJsonAsync("/api/auth", new LoginRequest(user.Email));

    var loginOk = await res.Content.ReadFromJsonAsync<LoginResponse.Ok>();
    Assert.NotNull(loginOk);

    await client.PostAsJsonAsync("/api/auth/confirm", new ConfirmRequest(user.Email, loginOk.Code));
  }
}
