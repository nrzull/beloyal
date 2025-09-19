using Beloyal.API.Bonuses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Beloyal.API.Test;

public class TestWebApplicationFactory() : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    base.ConfigureWebHost(builder);

    builder.ConfigureServices(services =>
    {
      var lifetimeService = services.FirstOrDefault(v =>
        v.ImplementationType == typeof(BonusLifetimeBackgroundService)
      );

      if (lifetimeService is { })
      {
        services.Remove(lifetimeService);
      }
    });

    builder.ConfigureAppConfiguration(
      (_ctx, conf) =>
      {
        var appSettings = new Dictionary<string, string?>
        {
          { "Logging:LogLevel:Default", LogLevel.Information.ToString() },
          { "Logging:LogLevel:Microsoft.EntityFrameworkCore", LogLevel.Critical.ToString() },
        };

        conf.AddInMemoryCollection(appSettings);
      }
    );
  }
}
