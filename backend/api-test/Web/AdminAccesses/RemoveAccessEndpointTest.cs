namespace Beloyal.API.Test.Web.AdminAccesses;

public class RemoveAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Remove() => WithScope(async (scope, _) => { });
}
