using Vogen;

namespace Beloyal.API.Users;

[ValueObject<string>]
public partial struct TgUserId
{
  public static TgUserId From(long id) => From(id.ToString());
}
