using Vogen;

namespace Beloyal.API.Users;

[ValueObject<Guid>]
public partial struct UserId
{
  public static UserId From(User v) => From(v.Id);
};
