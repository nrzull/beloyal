using Vogen;

namespace Beloyal.API.Chats;

[ValueObject<Guid>]
public partial struct ChatMessageId
{
  public static ChatMessageId From(Chat v) => From(v.Id);
};
