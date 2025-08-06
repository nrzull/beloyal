using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Shops;

public static class ShopFixture
{
  public static async Task<(Shop, ShopAccess)> Shop(
    AsyncServiceScope scope,
    Company company,
    CompanyAccess cAccess
  )
  {
    var shopContext = scope.Get<ShopContext>();

    var shop = await shopContext.CreateShop(company, "qwe");
    var sAccess = await shopContext.CreateAccess(shop, cAccess);

    return (shop, sAccess);
  }
}
