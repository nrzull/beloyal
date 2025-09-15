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
        var (company, cAccess, _) = await CompanyFixture.Company(
          scope,
          cashbackPercent: 10,
          cashbackThreshold: 10
        );

        var (shop, sAccess) = await ShopFixture.Shop(scope, company, cAccess);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var bonusContext = scope.Get<BonusContext>();

        var bonusRecord = await bonusContext.AddBonus(
          customer,
          sAccess,
          1000,
          DateTimeOffset.UtcNow
        );

        Assert.Equal(100, customer.BonusAmount);
      }
    );

  [Fact]
  public Task SubtractBonus() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, cAccess, _) = await CompanyFixture.Company(
          scope,
          cashbackPercent: 10,
          cashbackThreshold: 100
        );

        var (shop, sAccess) = await ShopFixture.Shop(scope, company, cAccess);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var bonusContext = scope.Get<BonusContext>();

        var bonusRecord = await bonusContext.AddBonus(
          customer,
          sAccess,
          1000,
          DateTimeOffset.UtcNow
        );
        Assert.Equal(100, customer.BonusAmount);

        var subtractBonusRecord = await bonusContext.SubtractBonus(
          customer,
          sAccess,
          75,
          DateTimeOffset.UtcNow.AddMinutes(1)
        );

        Assert.Equal(25, customer.BonusAmount);
        Assert.Equal(75, subtractBonusRecord.Amount);
        Assert.Equal(0, subtractBonusRecord.CashIncome);
      }
    );
}
