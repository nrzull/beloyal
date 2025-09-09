using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Test.Users;

public class UserContextTest : TestLifetime
{
  [Fact]
  public Task EnsureUser() =>
    WithScope(
      async (scope, _) =>
      {
        var userContext = scope.Get<UserContext>();
        var phone = new Phone("+123123123123");
        var user = await userContext.EnsureUser(phone);
        var user2 = await userContext.EnsureUser(phone);

        Assert.Equal(user.Id, user2.Id);
      }
    );
}
