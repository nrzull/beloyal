using Beloyal.API.Chats;

namespace Beloyal.API.Web.Chats.CreateMessage;

public abstract record CreateMessageResponse
{
  public record Ok(ChatMessageDTO Data) : CreateMessageResponse;
}
