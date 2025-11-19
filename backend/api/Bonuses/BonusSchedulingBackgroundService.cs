using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Bonuses;

public class BonusSchedulingBackgroundService(
  IServiceScopeFactory sf,
  ILogger<BonusSchedulingBackgroundService> logger
) : BackgroundService
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
      using var _ = logger.BeginScope("ReadNewBonuses");

      logger.LogInformation("ReadNewBonuses new bonus: {bonusId}", bonusId);

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
    using var _ = logger.BeginScope("RegisterJob");

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
        logger.LogInformation("RegisterJob new bonus: {bonusId}", bonus.Id);
        SheduleBonus(BonusId.From(bonus), scheduledAt, ct);
      }
    }
  }

  public void SheduleBonus(BonusId bonusId, DateTimeOffset scheduledAt, CancellationToken ct)
  {
    using var _ = logger.BeginScope("SheduleBonus");

    ct.ThrowIfCancellationRequested();

    logger.LogInformation("SheduleBonus new bonus: {bonusId}", bonusId.Value);

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
    using var _ = logger.BeginScope("ProcessBonus");

    try
    {
      logger.LogInformation("ProcessBonus new bonus: {bonusId}", bonusId.Value);

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
      logger.LogInformation("ProcessBonus save new bonus: {bonusId}", bonusId.Value);
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
    }
  }
}
