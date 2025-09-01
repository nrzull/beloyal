using Beloyal.API.Chats;

namespace Beloyal.API.Web.Chats.GetChatMessages;

public abstract record GetChatMessagesResponse
{
  public record Ok(List<ChatMessageDTO> Data) : GetChatMessagesResponse;
}
