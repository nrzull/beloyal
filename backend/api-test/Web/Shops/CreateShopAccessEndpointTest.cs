using Beloyal.API.Shops;
using Beloyal.API.Test.Companies;
using Beloyal.API.Web.Shops.CreateShopAccess;

namespace Beloyal.API.Test.Web.Shops;

public class CreateShopAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, user) = await CompanyFixture.Company(scope);
        var endpoint = scope.Get<CreateShopAccessEndpoint>();
        var shopContext = scope.Get<ShopContext>();
        var shop = await shopContext.CreateShop(company, "qwe", TimeZoneInfo.Local);

        var result = await endpoint.Handle(access, new(shop.Id, access.Id));
        Assert.IsType<CreateShopAccessResponse.Ok>(result);

        if (result is CreateShopAccessResponse.Ok ok)
        {
          Assert.Equal(ok.Data.Phone, user.Phone);
        }
      }
    );
}
