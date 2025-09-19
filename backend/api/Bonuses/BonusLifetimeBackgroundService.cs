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
    await using var scope = sf.CreateAsyncScope();
    var bonusContext = scope.ServiceProvider.GetRequiredService<BonusContext>();

    var bonuses = await bonusContext.GetLatestBonuses();
    var now = DateTimeOffset.UtcNow;

    PeriodicTimer time = new(TimeSpan.FromSeconds(10));

    while (await time.WaitForNextTickAsync(ct))
    {
      foreach (var bonus in bonuses)
      {
        if (bonus.Customer.BonusAmount <= 0)
        {
          continue;
        }

        if (bonus.Customer.Company.BonusLifetime is not { } bonusLifetime)
        {
          continue;
        }

        var deadline = bonus.CreatedAt.Add(bonusLifetime);
        if (deadline < now)
        {
          Console.WriteLine(
            $"Should burn {bonus.Customer.BonusAmount} for customer {bonus.Customer.Id}"
          );
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
