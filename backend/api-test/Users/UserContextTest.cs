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
        var email = new Email("qwe@gma.com");
        var user = await userContext.EnsureUser(email);
        var user2 = await userContext.EnsureUser(email);

        Assert.Equal(user.Id, user2.Id);
      }
    );
}
