using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Users;

public static class UserFixture
{
  public static async Task<User> User(AsyncServiceScope scope, string? phone = null)
  {
    phone ??= Environment.GetEnvironmentVariable("API_ADMIN_PHONE")!;
    var userContext = scope.Get<UserContext>();
    var user = await userContext.EnsureUser(new Phone(phone));
    return user;
  }
}
