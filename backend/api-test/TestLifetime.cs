using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test;

public class TestLifetime : IAsyncLifetime
{
  public TestWebApplicationFactory? App { get; set; }

  public Task InitializeAsync()
  {
    App = new TestWebApplicationFactory();
    return Task.CompletedTask;
  }

  public Task DisposeAsync() => Task.CompletedTask;

  public async Task WithScope(
    Func<AsyncServiceScope, ApplicationDbContext, Task> cb,
    [CallerMemberName] string caller = "",
    [CallerFilePath] string filepath = ""
  )
  {
    var nc = new Stopwatch();

    nc.Start();

    await using var scope = App!.Services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await using var tc = await dbContext.Database.BeginTransactionAsync();
    await cb(scope, dbContext);

    await tc.RollbackAsync();

    nc.Stop();

    if (nc.Elapsed.TotalSeconds > 5)
    {
      Console.WriteLine($"WARNING {caller} {filepath} elapsed: {nc.Elapsed.TotalSeconds}");
    }
  }
}
