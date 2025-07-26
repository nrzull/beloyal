using System.Diagnostics.CodeAnalysis;

namespace Beloyal.API.Users;

public static class UserExtensions
{
  public static void ThrowIfNull([NotNull] this User? user)
  {
    if (user is null)
    {
      throw new NullReferenceException();
    }
  }
}
