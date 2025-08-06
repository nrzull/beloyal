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

        var bonusRecord = await bonusContext.AddBonus(customer, sAccess, 1000);

        Assert.Equal(100, customer.Bonus);
      }
    );

  [Fact]
  public Task SubtractAndConfirmSubtractBonus() =>
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

        var bonusRecord = await bonusContext.AddBonus(customer, sAccess, 1000);
        Assert.Equal(100, customer.Bonus);

        var subtractBonusRecord = await bonusContext.SubtractBonus(customer, sAccess, 75);

        Assert.Equal(75, subtractBonusRecord.Income);

        Assert.Equal(BonusRecordStatus.NeedConfirmation, subtractBonusRecord.Status);

        Assert.IsType<string>(subtractBonusRecord.ConfirmationCode);

        var confirmedSubtractBonusRecord = await bonusContext.ConfirmSubtractBonus(
          BonusRecordId.From(subtractBonusRecord),
          subtractBonusRecord.ConfirmationCode
        );

        Assert.Equal(25, customer.Bonus);
        Assert.Equal(75, confirmedSubtractBonusRecord.Bonus);
        Assert.Equal(0, confirmedSubtractBonusRecord.Income);

        Assert.Null(confirmedSubtractBonusRecord.ConfirmationCode);
        Assert.Null(confirmedSubtractBonusRecord.ConfirmationCodeExpiredAt);
        Assert.Equal(BonusRecordStatus.Done, confirmedSubtractBonusRecord.Status);
      }
    );
}
