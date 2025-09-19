namespace Beloyal.API.Bonuses;

public class BonusLifetimeBackgroundService(IServiceScopeFactory sf) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    _ = ReadNewBonuses(ct);
    await ProcessExistingBonuses(ct);
  }

  public async Task ProcessExistingBonuses(CancellationToken ct)
  {
    PeriodicTimer time = new(TimeSpan.FromHours(1));

    while (await time.WaitForNextTickAsync(ct))
    {
      await using var scope = sf.CreateAsyncScope();
      var bonusContext = scope.ServiceProvider.GetRequiredService<BonusContext>();

      var bonuses = await bonusContext.GetLatestBonuses();
      var now = DateTimeOffset.UtcNow;

      foreach (var bonus in bonuses)
      {
        if (bonus.Customer.BonusAmount <= 0)
        {
          continue;
        }

        var deadline = bonus.CreatedAt.Add(bonus.Customer.Company.BonusLifetime);
        if (deadline < now)
        {
          try
          {
            await bonusContext.BurnBonus(bonus.Customer);
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
          }
        }
      }
    }
  }

  public async Task ReadNewBonuses(CancellationToken ct)
  {
    await foreach (var bonusId in BonusContext.NewBonusChannel.Reader.ReadAllAsync(ct))
    {
      Console.WriteLine($"new bonus: {bonusId}");
    }
  }
}
