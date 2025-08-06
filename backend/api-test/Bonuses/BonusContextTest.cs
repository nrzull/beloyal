using Beloyal.API.Bonuses;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Customers;
using Beloyal.API.Test.Shops;

namespace Beloyal.API.Test.Bonuses;

public class BonusContextTest : TestLifetime
{
  [Fact]
  public Task AddBonus() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, cAccess, _) = await CompanyFixture.Company(scope);
        var (shop, sAccess) = await ShopFixture.Shop(scope, company, cAccess);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var bonusContext = scope.Get<BonusContext>();

        var bonusRecord = await bonusContext.AddBonus(customer, sAccess, 1000);

        Assert.Equal(100, customer.Bonus);
      }
    );
}
