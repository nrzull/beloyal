using Beloyal.API.Chats;

namespace Beloyal.API.Web.Chats.GetChats;

public abstract record GetChatsResponse
{
  public record Ok(List<ChatDTO> Data) : GetChatsResponse;
}
