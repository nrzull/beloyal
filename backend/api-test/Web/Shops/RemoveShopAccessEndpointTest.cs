using Beloyal.API.Shops;
using Beloyal.API.Test.Companies;
using Beloyal.API.Web.Shops.RemoveShopAccess;

namespace Beloyal.API.Test.Web.Shops;

public class RemoveShopAccessEndpointTest : TestLifetime
{
  [Fact]
  public Task Remove() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var shopContext = scope.Get<ShopContext>();
        var shop = await shopContext.CreateShop(company, "qwe");
        var shopAccess = await shopContext.CreateAccess(shop, access);

        var result = await scope.Get<RemoveShopAccessEndpoint>().Handle(new(shopAccess.Id));

        Assert.IsType<RemoveShopAccessResponse.Ok>(result);
      }
    );
}
