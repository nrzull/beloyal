using Beloyal.API.Bonuses;

namespace Beloyal.API.Levels;

public class LevelWorker(IServiceScopeFactory sf, ILogger<LevelWorker> logger) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    await Process(ct);
  }

  public async Task Process(CancellationToken ct)
  {
    PeriodicTimer time = new(TimeSpan.FromHours(1));

    while (await time.WaitForNextTickAsync(ct))
    {
      using var _ = logger.BeginScope("LevelWorker.Process");

      await using var scope = sf.CreateAsyncScope();

      var bonusContext = scope.ServiceProvider.GetRequiredService<BonusContext>();

      var levelContext = scope.ServiceProvider.GetRequiredService<LevelContext>();

      foreach (var bonus in await bonusContext.GetLatestBonuses())
      {
        try
        {
          var customer = bonus.Customer;
          var deadline = bonus.CreatedAt.Add(customer.Company.LevelLifetime);
          var now = DateTimeOffset.UtcNow;
          if (deadline < now)
          {
            await levelContext.DowngradeLevel(customer);
          }
        }
        catch (Exception e)
        {
          logger.LogError(e, e.Message);
        }
      }
    }
  }
}
