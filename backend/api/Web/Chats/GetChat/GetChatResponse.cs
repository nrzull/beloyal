using Beloyal.API.Chats;

namespace Beloyal.API.Web.Chats.GetChat;

public abstract record GetChatResponse
{
  public record Ok(ChatDTO Data) : GetChatResponse;

  public record NotFound : GetChatResponse;
}
