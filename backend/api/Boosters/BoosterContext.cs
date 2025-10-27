using Beloyal.API.Customers;
using Beloyal.API.Types;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Boosters;

public class BoosterContext(ApplicationDbContext dbContext)
{
  public async Task<Booster> AddBooster(
    Customer customer,
    Percent cashbackPercent,
    DateTimeOffset expiredAt
  )
  {
    var booster = new Booster
    {
      CashbackPercent = cashbackPercent.Value,
      ExpiredAt = expiredAt.ToUniversalTime(),
      Customer = customer,
    };
    dbContext.Add(booster);
    await dbContext.SaveChangesAsync();

    return booster;
  }

  public async Task<List<Booster>> GetBoosters(CustomerId customerId) =>
    await dbContext
      .Boosters.Include(v => v.Bonus)
      .Where(v => v.CustomerId == customerId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<Booster>> GetAvailableBoosters(CustomerId customerId) =>
    await dbContext
      .Boosters.Include(v => v.Bonus)
      .Where(v =>
        v.CustomerId == customerId.Value && v.Bonus == null && v.ExpiredAt > DateTimeOffset.UtcNow
      )
      .OrderByDescending(v => v.CashbackPercent)
      .ToListAsync();

  public async Task<Booster?> GetAvailableBooster(CustomerId customerId) =>
    await dbContext
      .Boosters.Include(v => v.Bonus)
      .OrderByDescending(v => v.CashbackPercent)
      .FirstOrDefaultAsync(v =>
        v.CustomerId == customerId.Value && v.Bonus == null && v.ExpiredAt > DateTimeOffset.UtcNow
      );
}
