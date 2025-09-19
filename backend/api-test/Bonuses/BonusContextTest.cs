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

  [Fact]
  public Task GetLatestBonuses() =>
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
        var (customer2, _) = await CustomerFixture.Customer(scope, company, phone: "+12312312312");
        var bonusContext = scope.Get<BonusContext>();

        await bonusContext.AddBonus(customer, sAccess, 1000, DateTimeOffset.UtcNow);

        var b1 = await bonusContext.AddBonus(customer, sAccess, 1000, DateTimeOffset.UtcNow);

        var b2 = await bonusContext.AddBonus(customer2, sAccess, 1000, DateTimeOffset.UtcNow);

        var latest = await bonusContext.GetLatestBonuses();

        Assert.Contains(latest, (v) => v.Id == b1.Id);
        Assert.Contains(latest, (v) => v.Id == b2.Id);
        Assert.Equal(2, latest.Count);
      }
    );
}
