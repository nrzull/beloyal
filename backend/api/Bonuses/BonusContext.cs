using System.Threading.Channels;
using Beloyal.API.Boosters;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Promotions;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Bonuses;

public class BonusContext(
  ApplicationDbContext dbContext,
  ShopContext shopContext,
  PromotionContext promotionContext,
  BoosterContext boosterContext
)
{
  public static Channel<Guid> NewBonusChannel { get; } = Channel.CreateUnbounded<Guid>();

  public async Task<Bonus?> GetBonus(CompanyId companyId, BonusId bonusId) =>
    await dbContext
      .Bonuses.Include(v => v.Customer)
      .SingleOrDefaultAsync(v => v.Id == bonusId.Value && v.Customer.CompanyId == companyId.Value);

  public async Task<List<Bonus>> GetBonuses(CompanyId companyId) =>
    await dbContext
      .Bonuses.Include(v => v.Customer)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.Company)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.Role)
      .Include(v => v.Shop)
      .Where(v => v.Customer.CompanyId == companyId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<Bonus>> GetBonuses(CompanyId companyId, CustomerId customerId) =>
    await dbContext
      .Bonuses.Include(v => v.Customer)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.Company)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.Role)
      .Include(v => v.Shop)
      .Where(v => v.Customer.CompanyId == companyId.Value && v.Customer.Id == customerId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

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

  public async Task CancelBonus(Bonus bonus)
  {
    if (bonus.Status == BonusStatus.Cancelled)
    {
      throw new Exception("bonus already cancelled");
    }

    await dbContext.Entry(bonus).Reference(v => v.Customer).LoadAsync();
    var customer = bonus.Customer;

    dbContext.Update(customer);
    dbContext.Update(bonus);

    var lastBonus = bonus.Status;
    bonus.Status = BonusStatus.Cancelled;

    if (lastBonus == BonusStatus.Done)
    {
      if (bonus.Type == BonusType.AddBonus)
      {
        customer.BonusAmount -= bonus.Amount;
      }
      else if (bonus.Type == BonusType.SubtractBonus || bonus.Type == BonusType.BurnBonus)
      {
        customer.BonusAmount += bonus.Amount;
      }
    }

    await dbContext.SaveChangesAsync();
  }

  public async Task<Bonus> BurnBonus(Customer customer)
  {
    if (customer.BonusAmount <= 0)
    {
      throw new Exception("customer does not have bonuses");
    }

    var bonus = new Bonus
    {
      Type = BonusType.BurnBonus,
      Customer = customer,
      Amount = customer.BonusAmount,
      Status = BonusStatus.Done,
    };

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
    await ThrowIfBonusFrequencyExceeded(customer, agent.Shop.Company);
    await dbContext.Entry(agent).Reference(v => v.CompanyAccess).LoadAsync();

    var shopPromotions = await shopContext.GetPromotions(
      CompanyId.From(customer),
      ShopId.From(agent)
    );

    var promotion = await promotionContext.GetAppliablePromotion(
      customer,
      agent.Shop.Company,
      agent.Shop,
      shopPromotions.Select(v => v.Promotion).ToList()
    );

    var booster = await boosterContext.GetAvailableBooster(CustomerId.From(customer));

    var cashbacks = new List<decimal?>()
    {
      booster?.CashbackPercent,
      promotion?.Variant.CashbackPercent,
      agent.Shop.Company.CashbackPercent,
    }
      .Where(v => v is decimal)
      .OrderByDescending(v => v)
      .ToList();

    var cashbackPercent = cashbacks.FirstOrDefault() ?? agent.Shop.Company.CashbackPercent;

    var cashbackThreshold = agent.Shop.Company.CashbackThreshold;
    var appliedBonus = income / 100 * cashbackPercent;

    DateTimeOffset? scheduledAt = agent.Shop.Company.BonusScheduling is TimeSpan { } bs
      ? DateTimeOffset.UtcNow.Add(bs)
      : null;

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
      Status = scheduledAt is { } ? BonusStatus.Scheduled : BonusStatus.Done,
      ScheduledAt = scheduledAt,
      Promotion = promotion is { } p ? BonusPromotion.From(p) : null,
      Booster = booster,
    };
    dbContext.Add(bonus);

    if (scheduledAt is null)
    {
      dbContext.Update(customer);
      customer.BonusAmount += appliedBonus;
    }

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
    await ThrowIfBonusFrequencyExceeded(customer, agent.Shop.Company);
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
      Status = BonusStatus.Done,
    };
    dbContext.Add(bonus);

    dbContext.Update(customer);
    customer.BonusAmount -= bonusAmount;

    await dbContext.SaveChangesAsync();
    await NewBonusChannel.Writer.WriteAsync(bonus.Id);

    return bonus;
  }

  async Task ThrowIfBonusFrequencyExceeded(Customer customer, Company company)
  {
    var freq = company.BonusFrequency ?? null;

    if (freq is { })
    {
      var from = DateTimeOffset.UtcNow.Subtract(freq.Frequency);

      var lastBonuses = await dbContext
        .Bonuses.Where(v =>
          v.CustomerId == customer.Id && v.CreatedAt >= from && v.Type != BonusType.BurnBonus
        )
        .CountAsync();

      if (lastBonuses >= freq.Count)
      {
        throw new Exception("maximum bonus frequency exceeded");
      }
    }
  }
}
