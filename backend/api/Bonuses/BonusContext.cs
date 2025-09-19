using System.Threading.Channels;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Bonuses;

public class BonusContext(ApplicationDbContext dbContext)
{
  public static Channel<Guid> NewBonusChannel { get; } = Channel.CreateUnbounded<Guid>();

  public async Task<List<Bonus>> GetLatestBonuses()
  {
    var bonuses = (
      await dbContext
        .Bonuses.Include(v => v.Customer)
        .ThenInclude(v => v.Company)
        .AsSplitQuery()
        .OrderByDescending(v => v.CreatedAt)
        .ToListAsync()
    )
      .DistinctBy(v => v.CustomerId)
      .ToList();

    return bonuses;
  }

  public async Task<Bonus> BurnBonus(Customer customer)
  {
    if (customer.BonusAmount <= 0)
    {
      throw new Exception("customer does not have bonuses");
    }

    var bonus = new Bonus { Type = BonusType.BurnBonus, Customer = customer };
    dbContext.Add(bonus);

    dbContext.Update(customer);
    customer.BonusAmount = 0;

    await dbContext.SaveChangesAsync();
    await NewBonusChannel.Writer.WriteAsync(bonus.Id);

    return bonus;
  }

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
        v.CustomerId == customer.Id && v.CreatedAt >= publicTokenIssuedAt
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

    var bonus = new Bonus
    {
      Type = BonusType.AddBonus,
      Customer = customer,
      Agent = agent.CompanyAccess,
      Shop = agent.Shop,
      CashIncome = income,
      Amount = appliedBonus,
      CashbackPercent = cashbackPercent,
      CashbackThreshold = cashbackThreshold,
    };

    dbContext.Add(bonus);

    dbContext.Update(customer);
    customer.BonusAmount += appliedBonus;

    await dbContext.SaveChangesAsync();
    await NewBonusChannel.Writer.WriteAsync(bonus.Id);

    return bonus;
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
        v.CustomerId == customer.Id && v.CreatedAt >= publicTokenIssuedAt
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

    var bonusAmount =
      customer.BonusAmount > incomeThreshold ? incomeThreshold : customer.BonusAmount;

    var bonus = new Bonus
    {
      Type = BonusType.SubtractBonus,
      Customer = customer,
      Shop = agent.Shop,
      Amount = bonusAmount,
      Agent = agent.CompanyAccess,
      CashIncome = income - bonusAmount,
      CashbackPercent = cashbackPercent,
      CashbackThreshold = cashbackThreshold,
    };
    dbContext.Add(bonus);

    dbContext.Update(customer);
    customer.BonusAmount -= bonusAmount;

    await dbContext.SaveChangesAsync();
    await NewBonusChannel.Writer.WriteAsync(bonus.Id);

    return bonus;
  }
}
