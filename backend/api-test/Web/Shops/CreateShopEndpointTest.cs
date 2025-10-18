using Beloyal.API.Test.Companies;
using Beloyal.API.Web.Shops.CreateShop;

namespace Beloyal.API.Test.Web.Shops;

public class CreateShopEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var (_, access, _) = await CompanyFixture.Company(scope);
        var endpoint = scope.Get<CreateShopEndpoint>();

        var result = await endpoint.Handle(access, new("qwe", TimeZoneInfo.Local.Id));
        Assert.IsType<CreateShopResponse.Ok>(result);

        if (result is CreateShopResponse.Ok ok)
        {
          Assert.Equal("qwe", ok.Data.Name);
        }
      }
    );
}
