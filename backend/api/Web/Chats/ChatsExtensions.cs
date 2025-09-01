using Beloyal.API.Web.Chats.GetChatMessages;
using Beloyal.API.Web.Chats.GetChats;

namespace Beloyal.API.Web.Chats;

public static class ChatsExtensions
{
  public static void UseChatsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetChatsEndpoint>();
    builder.Services.AddScoped<GetChatMessagesEndpoint>();
  }
}
