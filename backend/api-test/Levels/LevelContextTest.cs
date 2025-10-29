using Beloyal.API.Levels;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Customers;

namespace Beloyal.API.Test.Levels;

public class LevelContextTest : TestLifetime
{
  [Fact]
  public Task DowngradeLevel() =>
    WithScope(
      async (scope, dbContext) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var levelContext = scope.Get<LevelContext>();

        await levelContext.DowngradeLevel(customer);
        Assert.Equal(0, customer.LevelPointsAmount);

        await levelContext.CreateLevel(company, new(1), 500, null);
        await levelContext.CreateLevel(company, new(1), 1000, null);
        await levelContext.CreateLevel(company, new(1), 1500, null);

        dbContext.Update(customer);
        customer.LevelPointsAmount = 2000;
        await dbContext.SaveChangesAsync();

        await levelContext.DowngradeLevel(customer);
        Assert.Equal(999, customer.LevelPointsAmount);

        await levelContext.DowngradeLevel(customer);
        Assert.Equal(499, customer.LevelPointsAmount);

        await levelContext.DowngradeLevel(customer);
        Assert.Equal(0, customer.LevelPointsAmount);

        await levelContext.DowngradeLevel(customer);
        Assert.Equal(0, customer.LevelPointsAmount);
      }
    );
}
