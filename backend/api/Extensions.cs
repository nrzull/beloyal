using System.Diagnostics.CodeAnalysis;
using Beloyal.API.Types;

namespace Beloyal.API;

public static class Extensions
{
  public static T Ensure<T>([NotNull] this T? entity)
    where T : IEnsurable
  {
    if (entity is null)
    {
      throw new NullReferenceException();
    }

    return entity;
  }
}
