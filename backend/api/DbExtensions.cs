using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Beloyal.API;

public static class DbExtensions
{
  public static async Task<(IDbContextTransaction?, bool)> EnsureTransactionAsync(
    this ApplicationDbContext dbContext
  )
  {
    var isInternal = dbContext.Database.CurrentTransaction is null;

    return (
      dbContext.Database.CurrentTransaction is null
        ? await dbContext.Database.BeginTransactionAsync()
        : null,
      isInternal
    );
  }

  public static async Task DisposeInternalAsync(this IDbContextTransaction? tc, bool isInternal)
  {
    if (tc is { } && isInternal)
    {
      await tc.CommitAsync();
      await tc.DisposeAsync();
    }
  }

  public static void SetupNpgsql(this WebApplicationBuilder builder)
  {
    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
      opts.UseNpgsql(ApplicationDbContext.GetConnectionString())
    );
  }
}
