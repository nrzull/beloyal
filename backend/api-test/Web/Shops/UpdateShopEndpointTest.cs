using Beloyal.API.Shops;
using Beloyal.API.Test.Companies;
using Beloyal.API.Web.Shops.UpdateShop;

namespace Beloyal.API.Test.Web.Shops;

public class UpdateShopEndpointTest : TestLifetime
{
  [Fact]
  public Task Update() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var endpoint = scope.Get<UpdateShopEndpoint>();
        var shopContext = scope.Get<ShopContext>();

        var shop = await shopContext.CreateShop(company, "qwe", TimeZoneInfo.Local);

        var result = await endpoint.Handle(
          access,
          ShopId.From(shop.Id),
          new("qweqwe", TimeZoneInfo.Local.Id)
        );

        Assert.IsType<UpdateShopResponse.Ok>(result);

        if (result is UpdateShopResponse.Ok ok)
        {
          Assert.Equal("qweqwe", ok.Data.Name);
        }
      }
    );
}
