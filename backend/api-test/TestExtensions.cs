using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test;

public static class TestExtensions
{
  public static T Get<T>(this AsyncServiceScope scope)
    where T : notnull => scope.ServiceProvider.GetRequiredService<T>();
}
