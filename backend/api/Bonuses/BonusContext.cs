using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Bonuses;

public class BonusContext(ApplicationDbContext dbContext)
{
  public async Task<Bonus> AddBonus(
    Customer customer,
    ShopAccess agent,
    decimal income,
    DateTimeOffset publicTokenIssuedAt
  )
  {
    if (income <= 0)
    {
      throw new Exception("invalid income");
    }

    if (
      await dbContext.Bonuses.AnyAsync(v =>
        v.CustomerId == customer.Id && v.PublicTokenIssuedAt >= publicTokenIssuedAt
      )
    )
    {
      throw new Exception("public token is stale");
    }

    await dbContext.Entry(agent).Reference(v => v.Shop).LoadAsync();
    await dbContext.Entry(agent.Shop).Reference(v => v.Company).LoadAsync();
    await dbContext.Entry(agent).Reference(v => v.CompanyAccess).LoadAsync();

    var cashbackPercent = agent.Shop.Company.CashbackPercent;
    var cashbackThreshold = agent.Shop.Company.CashbackThreshold;
    var appliedBonus = income / 100 * cashbackPercent;

    var bonusRecord = new Bonus
    {
      Type = BonusType.AddBonus,
      Customer = customer,
      Agent = agent.CompanyAccess,
      Shop = agent.Shop,
      CashIncome = income,
      Amount = appliedBonus,
      CashbackPercent = cashbackPercent,
      CashbackThreshold = cashbackThreshold,
      PublicTokenIssuedAt = publicTokenIssuedAt,
    };

    dbContext.Add(bonusRecord);

    dbContext.Update(customer);
    customer.BonusAmount += appliedBonus;

    await dbContext.SaveChangesAsync();

    return bonusRecord;
  }

  public async Task<Bonus> SubtractBonus(
    Customer customer,
    ShopAccess agent,
    decimal income,
    DateTimeOffset publicTokenIssuedAt
  )
  {
    if (income <= 0)
    {
      throw new Exception("invalid income");
    }

    if (customer.BonusAmount <= 0)
    {
      throw new Exception("invalid customer bonus");
    }

    if (
      await dbContext.Bonuses.AnyAsync(v =>
        v.CustomerId == customer.Id && v.PublicTokenIssuedAt >= publicTokenIssuedAt
      )
    )
    {
      throw new Exception("public token is stale");
    }

    await dbContext.Entry(agent).Reference(v => v.Shop).LoadAsync();
    await dbContext.Entry(agent.Shop).Reference(v => v.Company).LoadAsync();
    await dbContext.Entry(agent).Reference(v => v.CompanyAccess).LoadAsync();

    var cashbackPercent = agent.Shop.Company.CashbackPercent;
    var cashbackThreshold = agent.Shop.Company.CashbackThreshold;

    var incomeThreshold = income / 100 * cashbackThreshold;

    var bonus = customer.BonusAmount > incomeThreshold ? incomeThreshold : customer.BonusAmount;

    var bonusRecord = new Bonus
    {
      Type = BonusType.SubtractBonus,
      Customer = customer,
      Shop = agent.Shop,
      Amount = bonus,
      Agent = agent.CompanyAccess,
      CashIncome = income - bonus,
      CashbackPercent = cashbackPercent,
      CashbackThreshold = cashbackThreshold,
      PublicTokenIssuedAt = publicTokenIssuedAt,
    };
    dbContext.Add(bonusRecord);

    dbContext.Update(customer);
    customer.BonusAmount -= bonus;

    await dbContext.SaveChangesAsync();

    return bonusRecord;
  }
}
