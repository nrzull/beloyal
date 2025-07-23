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

  public async Task WithScope(Func<AsyncServiceScope, ApplicationDbContext, Task> cb)
  {
    await using var scope = App!.Services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await using var tc = await dbContext.Database.BeginTransactionAsync();
    await cb(scope, dbContext);
    await tc.RollbackAsync();
  }
}
