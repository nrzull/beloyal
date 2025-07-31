using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Beloyal.API;

public static class DbExtensions
{
  public class CustomTransaction(IDbContextTransaction? tc) : IAsyncDisposable
  {
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
      if (tc is { })
      {
        await tc.CommitAsync(cancellationToken);
      }
    }

    public async ValueTask DisposeAsync()
    {
      if (tc is { })
      {
        await tc.DisposeAsync();
      }
    }
  }

  public static async Task<CustomTransaction> MaybeTransactionAsync(
    this ApplicationDbContext dbContext
  )
  {
    var isInternal = dbContext.Database.CurrentTransaction is null;

    return new CustomTransaction(
      isInternal ? await dbContext.Database.BeginTransactionAsync() : null
    );
  }

  public static void SetupNpgsql(this WebApplicationBuilder builder)
  {
    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
      opts.UseNpgsql(ApplicationDbContext.GetConnectionString())
    );
  }
}
