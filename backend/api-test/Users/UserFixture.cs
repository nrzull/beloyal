using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Users;

public static class UserFixture
{
  public static async Task<User> User(AsyncServiceScope scope, string? email = null)
  {
    email ??= Environment.GetEnvironmentVariable("API_ADMIN_EMAIL")!;
    var userContext = scope.Get<UserContext>();
    var user = await userContext.EnsureUser(new Email(email));
    return user;
  }
}
