namespace Beloyal.API.Levels;

public class LevelWorker : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    // TODO: iterate over customer levels, check for timespan and extract points. Once it lower than prev level, downgrade
  }
}
