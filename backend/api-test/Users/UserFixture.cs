using Beloyal.API.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Users;

public static class UserFixture
{
  public static async Task<User> User(AsyncServiceScope scope, string email = "qwe@gma.com")
  {
    var userContext = scope.Get<UserContext>();
    var user = await userContext.EnsureUser(new(email));
    return user;
  }
}
