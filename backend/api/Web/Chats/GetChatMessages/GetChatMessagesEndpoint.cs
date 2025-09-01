using Beloyal.API.Chats;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Chats.GetChatMessages;

public class GetChatMessagesEndpoint(ChatContext chatContext)
{
  public async Task<GetChatMessagesResponse> Handle(CompanyAccess companyAccess, ChatId chatId)
  {
    var messages = await chatContext.GetChatMessages(CompanyId.From(companyAccess), chatId);

    return new GetChatMessagesResponse.Ok(messages.Select(ChatMessageDTO.From).ToList());
  }
}
