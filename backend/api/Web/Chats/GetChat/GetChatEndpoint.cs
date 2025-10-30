using Amazon.S3;
using Beloyal.API.Chats;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Chats.GetChat;

public class GetChatEndpoint(ChatContext chatContext, AmazonS3Client s3Client)
{
  public async Task<GetChatResponse> Handle(CompanyAccess companyAccess, ChatId chatId)
  {
    var chat = await chatContext.GetChatWithLastMessage(
      CompanyId.From(companyAccess),
      chatId,
      false
    );

    if (chat is null)
    {
      return new GetChatResponse.NotFound();
    }

    return new GetChatResponse.Ok(ChatDTO.From(chat, s3Client));
  }
}
