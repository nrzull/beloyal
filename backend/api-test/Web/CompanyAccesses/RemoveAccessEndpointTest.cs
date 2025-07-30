namespace Beloyal.API.Test.Web.CompanyAccesses;

public class RemoveAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Remove() => WithScope(async (scope, _) => { });
}
