using Vogen;

namespace Beloyal.API.Chats;

[ValueObject<Guid>]
public partial struct ChatId
{
  public static ChatId From(Chat v) => From(v.Id);
};
