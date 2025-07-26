using Beloyal.API.Shops;
using Beloyal.API.Test.Companies;

namespace Beloyal.API.Test.Shops;

public class ShopContextTest : TestLifetime
{
  [Fact]
  public Task CreateShop() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var shopContext = scope.Get<ShopContext>();

        var shop = await shopContext.CreateShop(company, "qwe");
        Assert.Equal(shop.CompanyId, company.Id);
      }
    );

  [Fact]
  public Task UpdateShop() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var shopContext = scope.Get<ShopContext>();

        var shop = await shopContext.CreateShop(company, "qwe");
        await shopContext.UpdateShop(shop, "qweqwe");
        Assert.Equal("qweqwe", shop.Name);
      }
    );

  [Fact]
  public Task CreateAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var shopContext = scope.Get<ShopContext>();
        var shop = await shopContext.CreateShop(company, "qwe");

        var shopAccess = await shopContext.CreateAccess(shop, access);
        Assert.Equal(access.Id, shopAccess.CompanyAccessId);
        Assert.Equal(shop.Id, shopAccess.ShopId);
      }
    );

  [Fact]
  public Task RemoveAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var shopContext = scope.Get<ShopContext>();
        var shop = await shopContext.CreateShop(company, "qwe");
        var shopAccess = await shopContext.CreateAccess(shop, access);
        await shopContext.RemoveAccess(shopAccess);

        Assert.DoesNotContain(await shopContext.GetAccesses(shop), v => v.Id == shopAccess.Id);
      }
    );

  [Fact]
  public Task GetAccessesByShop() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var shopContext = scope.Get<ShopContext>();
        var shop = await shopContext.CreateShop(company, "qwe");
        var shopAccess = await shopContext.CreateAccess(shop, access);

        Assert.Contains(await shopContext.GetAccesses(shop), v => v.Id == shopAccess.Id);
      }
    );
}
