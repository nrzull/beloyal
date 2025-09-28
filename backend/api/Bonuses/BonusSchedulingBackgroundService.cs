using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Bonuses;

public class BonusSchedulingBackgroundService(IServiceScopeFactory sf) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    _ = ReadNewBonuses(ct);
    await RegisterJob(ct, null);
    var now = DateTimeOffset.UtcNow;

    await using var scope = sf.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var bonuses = await dbContext
      .Bonuses.Where(v =>
        v.Status == BonusStatus.Scheduled && v.ScheduledAt < now && v.Type == BonusType.AddBonus
      )
      .ToListAsync(ct);

    foreach (var bonus in bonuses)
    {
      await ProcessBonus(BonusId.From(bonus), ct);
    }
  }

  public async Task ReadNewBonuses(CancellationToken ct)
  {
    await foreach (var bonusId in BonusContext.NewBonusChannel.Reader.ReadAllAsync(ct))
    {
      Console.WriteLine($"ReadNewBonuses new bonus: {bonusId}");

      await using var scope = sf.CreateAsyncScope();

      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      var now = DateTimeOffset.UtcNow;

      var bonus = await dbContext.Bonuses.SingleOrDefaultAsync(
        v =>
          v.Id == bonusId
          && v.ScheduledAt >= now
          && v.Status == BonusStatus.Scheduled
          && v.Type == BonusType.AddBonus,
        ct
      );

      if (bonus is { })
      {
        await RegisterJob(ct, bonus);
      }
    }
  }

  public async Task RegisterJob(CancellationToken ct, Bonus? bonusToRegister)
  {
    var now = DateTimeOffset.UtcNow;

    await using var scope = sf.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    List<Bonus> bonuses = bonusToRegister is { }
      ? [bonusToRegister]
      : await dbContext
        .Bonuses.Where(v =>
          v.Status == BonusStatus.Scheduled && v.ScheduledAt >= now && v.Type == BonusType.AddBonus
        )
        .ToListAsync(ct);

    foreach (var bonus in bonuses)
    {
      if (bonus.ScheduledAt is { } scheduledAt)
      {
        Console.WriteLine($"RegisterJob new bonus: {bonus.Id}");
        SheduleBonus(BonusId.From(bonus), scheduledAt, ct);
      }
    }
  }

  public void SheduleBonus(BonusId bonusId, DateTimeOffset scheduledAt, CancellationToken ct)
  {
    ct.ThrowIfCancellationRequested();

    Console.WriteLine($"SheduleBonus new bonus: {bonusId.Value}");

    var delay = scheduledAt - DateTimeOffset.UtcNow;
    Timer timer = new Timer(
      async (_) => await ProcessBonus(bonusId, ct),
      null,
      delay,
      Timeout.InfiniteTimeSpan
    );
  }

  async Task ProcessBonus(BonusId bonusId, CancellationToken ct)
  {
    try
    {
      Console.WriteLine($"ProcessBonus new bonus: {bonusId.Value}");

      ct.ThrowIfCancellationRequested();

      await using var scope = sf.CreateAsyncScope();

      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      var bonus = await dbContext.Bonuses.SingleAsync(v => v.Id == bonusId.Value);
      if (bonus.Status != BonusStatus.Scheduled)
      {
        return;
      }

      var customer = await dbContext.Customers.Where(v => v.Id == bonus.CustomerId).SingleAsync();

      dbContext.Update(customer);
      dbContext.Update(bonus);
      if (bonus.Type == BonusType.AddBonus)
      {
        customer.BonusAmount += bonus.Amount;
      }
      bonus.Status = BonusStatus.Done;

      await dbContext.SaveChangesAsync();
      Console.WriteLine($"ProcessBonus save new bonus: {bonusId.Value}");
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
    }
  }
}
