namespace Beloyal.API.Test.Mailings;

public class MailingContextTest : TestLifetime
{
  [Fact]
  public Task Create() => WithScope(async (scope, _) => { });
}
